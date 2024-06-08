using ires_api.DTO.Project;
using ires_api.Enumerations;

namespace ires_api.DTO.RentalUnit
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

        public RentalProjectViewModel? project { get; set; }
    }
}
