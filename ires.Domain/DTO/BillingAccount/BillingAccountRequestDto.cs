using ires.Domain.Enumerations;

namespace ires.Domain.DTO.BillingAccount
{
    public class BillingAccountRequestDto
    {
        public long Id { get; set; }
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
        public string Memo { get; set; } = string.Empty;
        public long CreatedById { get; set; }
        public long UpdatedById { get; set; }
    }
}
