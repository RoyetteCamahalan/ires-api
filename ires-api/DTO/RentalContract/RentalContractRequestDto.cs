using ires_api.DTO.RentalContractDetail;

namespace ires_api.DTO.RentalContract
{
    public class RentalContractRequestDto
    {
        public long contractid { get; set; }
        public long companyid { get; set; }
        public long contractno { get; set; }
        public long custid { get; set; }
        public DateTime contractdate { get; set; }
        public decimal montlyrent { get; set; }
        public int? term { get; set; }
        public decimal ewtpercentage { get; set; }
        public decimal? monthlypenalty { get; set; }
        public int? penaltyextension { get; set; }
        public DateTime? billingstart { get; set; }
        public string remarks { get; set; } = string.Empty;
        public long createdbyid { get; set; }
        public List<RentalContractDetailRequestDto> rentalContractDetails { get; set; } = new List<RentalContractDetailRequestDto>();
    }
}
