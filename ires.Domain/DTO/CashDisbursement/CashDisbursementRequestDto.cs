using ires.Domain.Enumerations;

namespace ires.Domain.DTO.CashDisbursement
{
    public class CashDisbursementRequestDto
    {
        public long disbursementid { get; set; }
        public long accountid { get; set; }
        public DateTime? refdate { get; set; }
        public string refno { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public string remarks { get; set; } = string.Empty;
        public DisbursementTransType transtype { get; set; }
        public long? refaccountid { get; set; } = 0;
        public long refdisbursementid { get; set; }
        public DisbursementStatus status { get; set; }
    }
}
