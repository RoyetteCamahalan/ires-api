namespace ires.Domain.Models
{
    public class RentalContractDetail
    {
        public long id { get; set; }
        public long contractid { get; set; }
        public long propertyid { get; set; }
        public long createdbyid { get; set; }
        public DateTime? datecreated { get; set; }
        public DateTime? deleted_at { get; set; }

        public RentalContract? rentalContract { get; set; }
        public RentalProperty? rentalProperty { get; set; }
    }
}
