using ires.Domain.DTO.Expense;
using ires.Domain.Enumerations;

namespace ires.Domain.DTO.BillingPayment
{
    public class BillingPaymentViewModel
    {
        public long Id { get; set; }
        public int CompanyId { get; set; }
        public long BillingAccountId { get; set; }
        public long? ExpenseId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public DateTime? NextDueDate { get; set; }
        public string RefNo { get; set; } = string.Empty;
        public string PaymentMode { get; set; } = string.Empty;
        public string Status { get; set; } = BillingPaymentStatus.Paid;
        public string Remarks { get; set; } = string.Empty;
        public long CreatedById { get; set; }
        public DateTime? DateCreated { get; set; }

        public BillingAccount.BillingAccountViewModel? BillingAccount { get; set; }
        public ExpenseViewModel? Expense { get; set; }
    }
}
