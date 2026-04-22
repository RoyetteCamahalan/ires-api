using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("billing_payments")]
    public class BillingPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public long UpdatedById { get; set; }
        public DateTime? DateUpdated { get; set; }

        public BillingAccount? BillingAccount { get; set; }
        public Expense? Expense { get; set; }
    }
}
