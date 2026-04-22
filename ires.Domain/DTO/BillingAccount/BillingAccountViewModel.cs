using ires.Domain.Enumerations;
using ires.Domain.DTO.ExpenseType;
using ires.Domain.DTO.Office;
using ires.Domain.DTO.Vendor;

namespace ires.Domain.DTO.BillingAccount
{
    public class BillingAccountViewModel
    {
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
        public DateTime? NextDueDate { get; set; }

        public VendorViewModel? Vendor { get; set; }
        public OfficeViewModel? Office { get; set; }
        public ExpenseTypeViewModel? ExpenseType { get; set; }
    }
}
