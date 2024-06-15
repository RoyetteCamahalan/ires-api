using ires.Domain.DTO.CashDisbursement;

namespace ires.Domain.Contracts
{
    public interface IPettyCashService
    {
        public Task<CashDisbursementViewModel> Create(CashDisbursementRequestDto requestDto);
        public Task<CashDisbursementViewModel> GetDisbursementByID(long id);
        public Task<ICollection<CashDisbursementViewModel>> GetCashDisbursements(int companyID, string search, DateTime startDate, DateTime endDate);
        public Task<decimal> TotalPettyCashBalance(int companyID);
        public Task<bool> VoidDisbursement(long id, bool isRefDisbursement);
        public Task ReComputePettyCash(long accountID);
    }
}
