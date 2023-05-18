using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IPettyCashService
    {
        public Task<CashDisbursement> Create(CashDisbursement cashDisbursement);
        public Task<CashDisbursement> GetDisbursementByID(long id);
        public Task<ICollection<CashDisbursement>> GetCashDisbursements(int companyID, string search, DateTime startDate, DateTime endDate);
        public Task<decimal> TotalPettyCashBalance(int companyID);
        public Task<bool> VoidDisbursement(long id, bool isRefDisbursement);
        public Task ReComputePettyCash(long accountID);
    }
}
