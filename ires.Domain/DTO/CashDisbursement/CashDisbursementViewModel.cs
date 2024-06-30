using ires.Domain.DTO.Office;
using ires.Domain.Enumerations;

namespace ires.Domain.DTO.CashDisbursement
{
    public class CashDisbursementViewModel
    {
        public long disbursementid { get; set; }
        public int companyid { get; set; }
        public long accountid { get; set; }
        public DateTime? refdate { get; set; }
        public string refno { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public string remarks { get; set; } = string.Empty;
        public DisbursementTransType transtype { get; set; }
        public long? refaccountid { get; set; }
        public long refdisbursementid { get; set; }
        public DisbursementStatus status { get; set; }
        public OfficeViewModel? office { get; set; }
        public OfficeViewModel? refOffice { get; set; }
    }
}
