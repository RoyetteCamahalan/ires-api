using ires.Domain.Enumerations;

namespace ires.Domain.Models
{
    public class Booking
    {
        public long id { get; set; }
        public Guid guid { get; set; }
        public int companyid { get; set; }
        public long clientid { get; set; }
        public long carid { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public int noofdays { get; set; }
        public BookingRateType ratetype { get; set; }
        public decimal rate { get; set; }
        public decimal totalrate { get; set; }
        public string drivername { get; set; } = string.Empty;
        public bool isselfdrive { get; set; }
        public BookingStatus status { get; set; }
        public string remarks { get; set; } = string.Empty;

        public Car car { get; set; } = new Car();
        public Client client { get; set; } = new Client();
    }
}
