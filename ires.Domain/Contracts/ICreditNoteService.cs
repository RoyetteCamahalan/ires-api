using ires.Domain.Common;
using ires.Domain.DTO.CreditNote;

namespace ires.Domain.Contracts
{
    public interface ICreditNoteService
    {
        public Task<CreditMemoTypeViewModel> CreateType(CreditMemoTypeRequestDto requestDto);
        public Task UpdateType(CreditMemoTypeRequestDto requestDto);
        public Task<CreditMemoTypeViewModel> GetType(long id);
        public Task<CreditMemoTypeViewModel> GetTypeByName(string name);
        public Task<PaginatedResult<CreditMemoTypeViewModel>> GetTypes(PaginationRequest request);
    }
}
