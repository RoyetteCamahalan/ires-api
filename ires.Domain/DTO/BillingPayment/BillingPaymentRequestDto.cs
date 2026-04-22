using ires.Domain.Enumerations;

namespace ires.Domain.DTO.BillingPayment
{
    public class BillingPaymentRequestDto
    {
        public long Id { get; set; }
        public long BillingAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public DateTime? NextDueDate { get; set; }
        public string RefNo { get; set; } = string.Empty;
        public string PaymentMode { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public long CreatedById { get; set; }
        public long UpdatedById { get; set; }
        public bool? UsePettyCash { get; set; }
    }
}
