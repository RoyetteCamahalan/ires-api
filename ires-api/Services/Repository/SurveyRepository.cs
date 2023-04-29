using ires_api.Data;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
{
    public class SurveyRepository : ISurveyService
    {
        private readonly DataContext _dataContext;

        public SurveyRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public Survey Create(Survey survey)
        {
            survey.id = 0;
            survey.datecreated = DateTime.Now;
            survey.client = null;
            survey.balance = survey.contractprice;
            _dataContext.surveys.Add(survey);
            _dataContext.SaveChanges();
            return survey;
        }

        public Survey GetSurveyByID(long id)
        {
            return _dataContext.surveys.Include(x => x.client).Single(x => x.id == id);
        }

        public ICollection<Survey> GetSurveys(long companyID, string search)
        {
            return _dataContext.surveys.Include(x => x.client).Where(x => x.companyid == companyID &&
                (x.propertyname.Contains(search) || x.address.Contains(search) || (x.client.fname ?? "").Contains(search) || (x.client.lname ?? "").Contains(search)))
                .OrderByDescending(x => x.datecreated).ToList();
        }

        public Survey Update(SurveyRequestDto requestDto)
        {
            Survey survey = GetSurveyByID(requestDto.id);
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
                survey.balance = survey.balance + (requestDto.contractprice - survey.contractprice);
                survey.contractprice = requestDto.contractprice;
                survey.updatedbyid = requestDto.updatedbyid;
                survey.dateupdated = DateTime.Now;
                if (survey.balance > 0 && survey.status == Constants.SurveyStatus.completed)
                    survey.status = Constants.SurveyStatus.surveyed;
                _dataContext.SaveChanges();
            }
            return survey;
        }

        public Survey UpdateStatus(long ID, int status)
        {
            Survey survey = GetSurveyByID(ID);
            if (survey != null)
            {
                if (status == Constants.SurveyStatus.surveyed && survey.balance <= 0)
                    status = Constants.SurveyStatus.completed;

                survey.status = status;
                _dataContext.SaveChanges();
            }
            return survey;
        }
    }
}
