using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("billing_accounts")]
    public class BillingAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int CompanyId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public long VendorId { get; set; }
        public long OfficeId { get; set; }
        public long ExpenseTypeId { get; set; }
        public bool HasFixedAmount { get; set; }
        public decimal? Amount { get; set; }
        public string Frequency { get; set; } = BillingFrequency.Monthly;
        public int? DueDayOfMonth { get; set; }
        public int? DueDayOfWeek { get; set; }
        public int NotifyDaysBefore { get; set; }
        public string NotifyOption { get; set; } = BillingNotification.OnlyMe;
        public bool IsActive { get; set; }
        public string Memo { get; set; } = string.Empty;
        public long CreatedById { get; set; }
        public DateTime? DateCreated { get; set; }
        public long UpdatedById { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? NextDueDate { get; set; }
        public DateTime? LastNotified { get; set; }

        public Vendor? Vendor { get; set; }
        public Office? Office { get; set; }
        public ExpenseType? ExpenseType { get; set; }
        public List<BillingPayment> BillingPayments { get; set; } = new();
    }
}
