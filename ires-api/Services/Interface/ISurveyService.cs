using ires_api.DTO.Survey;

namespace ires_api.Services.Interface
{
    public interface ISurveyService
    {
        public Task<SurveyViewModel> Create(SurveyRequestDto requestDto);
        public Task<bool> Update(SurveyRequestDto requestDto);
        public Task<bool> UpdateStatus(long ID, int status);
        public Task<ICollection<SurveyViewModel>> GetSurveys(long companyID, string search);
        public Task<SurveyViewModel> GetByID(long id);
        public Task<int> CountPending(long companyID);
        public Task<int> CountCompleted(long companyID);

    }
}
