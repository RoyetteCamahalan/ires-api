using ires.Domain.DTO.BillingAccount;
using ires.Domain.DTO.BillingPayment;

namespace ires.Domain.Contracts
{
    public interface IBillingAccountService
    {
        // Billing Accounts
        Task<BillingAccountViewModel> CreateBillingAccount(BillingAccountRequestDto requestDto);
        Task<bool> UpdateBillingAccount(BillingAccountRequestDto requestDto);
        Task<BillingAccountViewModel?> GetBillingAccountById(long id);
        Task<ICollection<BillingAccountViewModel>> GetBillingAccounts(bool viewAll, string search);
        Task<bool> SetBillingAccountStatus(long id, bool isActive);

        // Billing Payments
        Task<BillingPaymentViewModel> PostPayment(BillingPaymentRequestDto requestDto);
        Task<bool> VoidPayment(long billingPaymentId);
        Task<BillingPaymentViewModel?> GetBillingPaymentById(long id);
        Task<ICollection<BillingPaymentViewModel>> GetBillingPayments(long billingAccountId);
    }
}
