using AutoMapper;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO.Survey;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class SurveyRepository : ISurveyService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public SurveyRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<int> CountCompleted(int companyID)
        {
            return await _dataContext.surveys.Where(x => x.companyid == companyID && x.status == SurveyStatus.completed).CountAsync();
        }

        public async Task<int> CountPending(int companyID)
        {
            return await _dataContext.surveys.Where(x => x.companyid == companyID && x.status == SurveyStatus.pending).CountAsync();
        }

        public async Task<SurveyViewModel> Create(SurveyRequestDto requestDto)
        {
            var entity = _mapper.Map<Survey>(requestDto);
            entity.id = 0;
            entity.datecreated = Utility.GetServerTime();
            entity.client = null;
            entity.balance = entity.contractprice;
            _dataContext.surveys.Add(entity);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<SurveyViewModel>(entity);
        }

        private async Task<Survey> GetSurveyByID(long id)
        {
            return await _dataContext.surveys.Include(x => x.client).SingleAsync(x => x.id == id);
        }
        public async Task<SurveyViewModel> GetByID(long id)
        {
            var result = await _dataContext.surveys.Include(x => x.client).SingleAsync(x => x.id == id);
            return _mapper.Map<SurveyViewModel>(result);
        }

        public async Task<ICollection<SurveyViewModel>> GetSurveys(int companyID, string search)
        {
            var result = await _dataContext.surveys.Include(x => x.client).Where(x => x.companyid == companyID &&
                (x.propertyname.Contains(search) || x.address.Contains(search) || (x.client.fname ?? "").Contains(search) || (x.client.lname ?? "").Contains(search)))
                .OrderByDescending(x => x.datecreated).ToListAsync();
            return _mapper.Map<ICollection<SurveyViewModel>>(result);
        }

        public async Task<bool> Update(SurveyRequestDto requestDto)
        {
            Survey survey = await GetSurveyByID(requestDto.id);
            if (survey != null)
            {
                survey.custid = requestDto.custid;
                survey.owner = requestDto.owner;
                survey.titleno = requestDto.titleno;
                survey.surveyno = requestDto.surveyno;
                survey.surveydate = requestDto.surveydate;
                survey.propertyname = requestDto.propertyname;
                survey.address = requestDto.address;
                survey.details = requestDto.details;
                survey.balance += requestDto.contractprice - survey.contractprice;
                survey.contractprice = requestDto.contractprice;
                survey.landarea = requestDto.landarea;
                survey.updatedbyid = requestDto.updatedbyid;
                survey.dateupdated = Utility.GetServerTime();
                if (survey.balance > 0 && survey.status == SurveyStatus.completed)
                    survey.status = SurveyStatus.surveyed;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateStatus(long ID, SurveyStatus status)
        {
            Survey survey = await GetSurveyByID(ID);
            if (survey != null)
            {
                if (status == SurveyStatus.surveyed && survey.balance <= 0)
                    status = SurveyStatus.completed;

                survey.status = status;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
