using System.ComponentModel.DataAnnotations;

namespace ires.AppService.Dto.RentalCharge
{
    public class UpdateRentalChargeRequestDto
    {
        [Required]
        public long chargeid { get; set; }
        [Required]
        public long otherfeeid { get; set; }
        [Required]
        public DateTime chargedate { get; set; }
        [Required]
        public decimal chargeamount { get; set; }
        public decimal interestpercentage { get; set; }
    }
}
