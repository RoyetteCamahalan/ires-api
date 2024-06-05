using ires_api.DTO.Survey;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface ISurveyService
    {
        public Task<Survey> Create(Survey survey);
        public Task<Survey> Update(SurveyRequestDto requestDto);
        public Task<Survey> UpdateStatus(long ID, int status);
        public Task<ICollection<Survey>> GetSurveys(long companyID, string search);
        public Task<Survey> GetSurveyByID(long id);
        public Task<int> CountPending(long companyID);
        public Task<int> CountCompleted(long companyID);

    }
}
