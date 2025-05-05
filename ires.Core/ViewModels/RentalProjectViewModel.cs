
using ires.Domain.DTO.RentalUnit;

namespace ires.Core.ViewModels
{
    public class RentalProjectViewModel
    {
        public long propertyid { get; set; }
        public int companyid { get; set; }
        public string propertyname { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public int projectypeid { get; set; }
        public int noofunits { get; set; }
        public int noofoccupiedunits { get; set; }
        public List<RentalUnitViewModel> rentalProperties { get; set; } = [];
    }
}
