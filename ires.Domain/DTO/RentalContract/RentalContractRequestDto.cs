using ires.Domain.DTO.RentalContractDetail;

namespace ires.Domain.DTO.RentalContract
{
    public class RentalContractRequestDto
    {
        public long contractid { get; set; }
        public int companyid { get; set; }
        public long contractno { get; set; }
        public long custid { get; set; }
        public DateTime contractdate { get; set; }
        public decimal montlyrent { get; set; }
        public decimal? deposit { get; set; }
        public int? term { get; set; }
        public int? noofmonthadvance { get; set; }
        public decimal ewtpercentage { get; set; }
        public decimal? monthlypenalty { get; set; }
        public int? penaltyextension { get; set; }
        public int billingsched { get; set; } = 1;
        public string remarks { get; set; } = string.Empty;
        public long createdbyid { get; set; }
        public long updatedbyid { get; set; }
        public List<RentalContractDetailRequestDto> rentalContractDetails { get; set; } = new List<RentalContractDetailRequestDto>();
    }
}
