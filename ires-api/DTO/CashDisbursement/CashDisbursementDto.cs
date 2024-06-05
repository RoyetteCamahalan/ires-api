using ires_api.DTO.Office;

namespace ires_api.DTO.CashDisbursement
{
    public class CashDisbursementDto
    {
        public long disbursementid { get; set; }
        public int companyid { get; set; }
        public long accountid { get; set; }
        public DateTime? refdate { get; set; }
        public string refno { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public string remarks { get; set; } = string.Empty;
        public int transtype { get; set; }
        public long refaccountid { get; set; }
        public long refdisbursementid { get; set; }
        public int status { get; set; } //See Constants.DisbursementStatus
        public OfficeDto? office { get; set; }
        public OfficeRequestDto? refOffice { get; set; }
    }
}
