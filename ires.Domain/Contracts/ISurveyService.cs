using ires.Domain.Common;
using ires.Domain.DTO.Survey;
using ires.Domain.Enumerations;

namespace ires.Domain.Contracts
{
    public interface ISurveyService
    {
        public Task<SurveyViewModel> Create(SurveyRequestDto requestDto);
        public Task Update(SurveyRequestDto requestDto);
        public Task UpdateStatus(long ID, SurveyStatus status);
        public Task<PaginatedResult<SurveyViewModel>> GetSurveys(PaginationRequest request);
        public Task<SurveyViewModel> GetByID(long id);
        public Task<int> CountPending();
        public Task<int> CountCompleted();

    }
}
