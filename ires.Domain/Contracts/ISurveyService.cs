using ires.Domain.DTO.Survey;
using ires.Domain.Enumerations;

namespace ires.Domain.Contracts
{
    public interface ISurveyService
    {
        public Task<SurveyViewModel> Create(SurveyRequestDto requestDto);
        public Task<bool> Update(SurveyRequestDto requestDto);
        public Task<bool> UpdateStatus(long ID, SurveyStatus status);
        public Task<ICollection<SurveyViewModel>> GetSurveys(long companyID, string search);
        public Task<SurveyViewModel> GetByID(long id);
        public Task<int> CountPending(long companyID);
        public Task<int> CountCompleted(long companyID);

    }
}
