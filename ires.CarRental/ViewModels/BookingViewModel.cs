using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace ires.Application.ViewModels
{
    public class BookingViewModel
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
        [MaxLength(250)]
        public string drivername { get; set; } = string.Empty;
        public bool isselfdrive { get; set; }
        public BookingStatus status { get; set; }
        public string remarks { get; set; } = string.Empty;

        public CarViewModel? car { get; set; }
        public ClientViewModel? client { get; set; }
    }
}
