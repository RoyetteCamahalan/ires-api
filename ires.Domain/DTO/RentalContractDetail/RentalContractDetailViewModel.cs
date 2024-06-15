using ires.Domain.DTO.RentalContract;
using ires.Domain.DTO.RentalUnit;

namespace ires.Domain.DTO.RentalContractDetail
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
