using ires.Domain.DTO.RentalUnit;

namespace ires.Application.ViewModels
{
    public class RentalContractDetailViewModel
    {
        public long id { get; set; }
        public long contractid { get; set; }
        public long propertyid { get; set; }

        public RentalContractViewModel? rentalContract { get; set; }
        public RentalUnitViewModel? rentalProperty { get; set; }
    }
}
