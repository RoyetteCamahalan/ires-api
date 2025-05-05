using ires.Domain.Enumerations;

namespace ires.Domain.Models
{
    public class RentalUnit
    {
        public long propertyid { get; set; }

        public long projectid { get; set; }
        public string propertyname { get; set; } = string.Empty;
        public string area { get; set; } = string.Empty;
        public decimal monthlyrent { get; set; } = 0;
        public string alias { get; set; } = string.Empty;
        public RentalPropertyStatus status { get; set; }

        public Project? project { get; set; }
    }
}
