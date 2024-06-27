using ires.Domain.DTO.CreditNote;

namespace ires.Domain.Contracts
{
    public interface ICreditNoteService
    {
        public Task<CreditMemoTypeViewModel> CreateType(CreditMemoTypeRequestDto requestDto);
        public Task<bool> UpdateType(CreditMemoTypeRequestDto requestDto);
        public Task<CreditMemoTypeViewModel> GetType(long id);
        public Task<CreditMemoTypeViewModel> GetTypeByName(int companyID, string name);
        public Task<ICollection<CreditMemoTypeViewModel>> GetTypes(int companyID, string search, bool viewAll);
    }
}
