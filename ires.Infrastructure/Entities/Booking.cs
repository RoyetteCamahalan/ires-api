using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("bookings")]
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public int companyid { get; set; }
        public long clientid { get; set; }
        public long carid { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public int noofdays { get; set; }
        public int ratetype { get; set; } //Constants.BookingRateType
        public decimal rate { get; set; }
        public decimal totalrate { get; set; }
        [MaxLength(250)]
        public string drivername { get; set; } = string.Empty;
        public bool isselfdrive { get; set; }
        public int status { get; set; } //Constants.BookingStatus
        public string remarks { get; set; } = string.Empty;

        public Car car { get; set; } = new Car();
        public Client client { get; set; } = new Client();
    }
}
