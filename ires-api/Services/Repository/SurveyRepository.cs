using AutoMapper;
using ires_api.Data;
using ires_api.DTO.Survey;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
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

        public async Task<int> CountCompleted(long companyID)
        {
            return await _dataContext.surveys.Where(x => x.companyid == companyID && x.status == Constants.SurveyStatus.completed).CountAsync();
        }

        public async Task<int> CountPending(long companyID)
        {
            return await _dataContext.surveys.Where(x => x.companyid == companyID && x.status == Constants.SurveyStatus.pending).CountAsync();
        }

        public async Task<SurveyViewModel> Create(SurveyRequestDto requestDto)
        {
            var entity = _mapper.Map<Survey>(requestDto);
            entity.id = 0;
            entity.datecreated = DateTime.Now;
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

        public async Task<ICollection<SurveyViewModel>> GetSurveys(long companyID, string search)
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
                survey.balance += (requestDto.contractprice - survey.contractprice);
                survey.contractprice = requestDto.contractprice;
                survey.updatedbyid = requestDto.updatedbyid;
                survey.dateupdated = DateTime.Now;
                if (survey.balance > 0 && survey.status == Constants.SurveyStatus.completed)
                    survey.status = Constants.SurveyStatus.surveyed;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateStatus(long ID, int status)
        {
            Survey survey = await GetSurveyByID(ID);
            if (survey != null)
            {
                if (status == Constants.SurveyStatus.surveyed && survey.balance <= 0)
                    status = Constants.SurveyStatus.completed;

                survey.status = status;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
