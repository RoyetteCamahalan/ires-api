using ires.Domain.Common;
using ires.Domain.DTO.CashDisbursement;
using ires.Domain.DTO.PettyCash;

namespace ires.Domain.Contracts
{
    public interface IPettyCashService
    {
        public Task<CashDisbursementViewModel> Create(CashDisbursementRequestDto requestDto);
        public Task<CashDisbursementViewModel> GetDisbursementByID(long id);
        public Task<PaginatedResult<CashDisbursementViewModel>> GetCashDisbursements(PaginationRequest request);
        public Task<decimal> TotalPettyCashBalance();
        public Task VoidDisbursement(long id, bool isRefDisbursement);
        public Task ReComputePettyCash(long accountID);

        public Task<ICollection<PettyCashAccountHistoryViewModel>> GetAccountHistory(long accountID, DateTime startDate, DateTime endDate);
    }
}
