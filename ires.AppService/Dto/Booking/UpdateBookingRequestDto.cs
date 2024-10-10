using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace ires.AppService.Dto.Booking
{
    public class UpdateBookingRequestDto
    {
        [Required]
        public long id { get; set; }
        [Required]
        public long clientid { get; set; }
        [Required]
        public long carid { get; set; }
        [Required]
        public DateTime startdate { get; set; }
        [Required]
        public DateTime enddate { get; set; }
        public int noofdays { get; set; }
        [Required]
        public BookingRateType ratetype { get; set; }
        [Required]
        public decimal rate { get; set; }
        [MaxLength(250)]
        public string drivername { get; set; } = string.Empty;
        public bool isselfdrive { get; set; }
        public string remarks { get; set; } = string.Empty;
    }
}
