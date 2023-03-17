using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface ISurveyService
    {
        public Survey Create(Survey survey);
        public Survey Update(SurveyRequestDto requestDto);
        public ICollection<Survey> GetSurveys(long companyID, string search);
        public Survey GetSurveyByID(long id);
        
    }
}
