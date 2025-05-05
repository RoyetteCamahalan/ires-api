using ires.Domain.DTO.Project;
using ires.Domain.Enumerations;

namespace ires.Domain.DTO.RentalUnit
{
    public class RentalUnitViewModel
    {
        public long propertyid { get; set; }

        public long projectid { get; set; }
        public string propertyname { get; set; } = string.Empty;
        public string area { get; set; } = string.Empty;
        public decimal monthlyrent { get; set; } = 0;
        public RentalPropertyStatus status { get; set; }
        public string tenant { get; set; } = string.Empty;
        public long contract_id { get; set; }

        public RentalProjectViewModel? project { get; set; }
    }
}
