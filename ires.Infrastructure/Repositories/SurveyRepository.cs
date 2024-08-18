using AutoMapper;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.Survey;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Infrastructure.Extensions;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class SurveyRepository(
        DataContext _dataContext,
        IMapper _mapper,
        ILogService _logService,
        ICurrentUserService _currentUserService) : ISurveyService
    {

        public async Task<int> CountCompleted()
        {
            return await _dataContext.surveys.Where(x => x.companyid == _currentUserService.companyid &&
                x.status == SurveyStatus.completed).CountAsync();
        }

        public async Task<int> CountPending()
        {
            return await _dataContext.surveys.Where(x => x.companyid == _currentUserService.companyid &&
                x.status == SurveyStatus.pending).CountAsync();
        }

        public async Task<SurveyViewModel> Create(SurveyRequestDto requestDto)
        {
            var entity = _mapper.Map<Survey>(requestDto);
            entity.id = 0;
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            entity.client = null;
            entity.balance = entity.contractprice;
            _dataContext.surveys.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Surveying, "Create Survey", "New Survey - " + entity.id);
            return _mapper.Map<SurveyViewModel>(entity);
        }

        private async Task<Survey> GetSurveyByID(long id)
        {
            return await _dataContext.surveys.Include(x => x.client).SingleAsync(x => x.id == id) ?? throw new EntityNotFoundException();
        }
        public async Task<SurveyViewModel> GetByID(long id)
        {
            var result = await GetSurveyByID(id);
            return _mapper.Map<SurveyViewModel>(result);
        }

        public async Task<PaginatedResult<SurveyViewModel>> GetSurveys(PaginationRequest request)
        {
            var query = _dataContext.surveys.Include(x => x.client).Where(x => x.companyid == _currentUserService.companyid &&
                (x.propertyname.Contains(request.searchString) || x.address.Contains(request.searchString) || (x.client.fname ?? "").Contains(request.searchString) || (x.client.lname ?? "").Contains(request.searchString)))
                .OrderByDescending(x => x.datecreated).AsQueryable();
            return await query.AsPaginatedResult<Survey, SurveyViewModel>(request, _mapper.ConfigurationProvider);
        }

        public async Task Update(SurveyRequestDto requestDto)
        {
            Survey survey = await GetSurveyByID(requestDto.id);
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
            survey.updatedbyid = _currentUserService.employeeid;
            survey.dateupdated = Utility.GetServerTime();
            if (survey.balance > 0 && survey.status == SurveyStatus.completed)
                survey.status = SurveyStatus.surveyed;
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Surveying, "Update Survey", "Survey ID- " + survey.id);
        }

        public async Task UpdateStatus(long ID, SurveyStatus status)
        {
            Survey survey = await GetSurveyByID(ID);
            if (status == SurveyStatus.surveyed && survey.balance <= 0)
                status = SurveyStatus.completed;

            survey.status = status;
            await _dataContext.SaveChangesAsync();
        }
    }
}
