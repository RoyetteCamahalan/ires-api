using ires.AppService.Dto.RentalContractDetail;
using System.ComponentModel.DataAnnotations;

namespace ires.Domain.DTO.RentalContract
{
    public class UpdateRentalContractRequestDto
    {
        [Required]
        public long contractid { get; set; }
        [Required]
        public long contractno { get; set; }
        [Required]
        public long custid { get; set; }
        [Required]
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
        public List<RentalContractDetailRequestDto> rentalContractDetails { get; set; } = [];
    }
}
