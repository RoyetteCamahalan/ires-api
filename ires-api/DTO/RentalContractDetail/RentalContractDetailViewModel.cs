using ires_api.DTO.RentalUnit;

namespace ires_api.DTO.RentalContractDetail
{
    public class RentalContractDetailViewModel
    {
        public long id { get; set; }
        public long contractid { get; set; }
        public long propertyid { get; set; }

        public RentalContractDetailViewModel? rentalContract { get; set; }
        public RentalUnitViewModel? rentalProperty { get; set; }
    }
}
