namespace ires.Domain.DTO.RentalCharge
{
    public class RentalChargeRequestDto
    {
        public long chargeid { get; set; }
        public long contractid { get; set; }
        public long otherfeeid { get; set; }
        public DateTime chargedate { get; set; }
        public decimal chargeamount { get; set; }
        public decimal interestpercentage { get; set; }
        public long createdbyid { get; set; }
        public long updatedbyid { get; set; }
    }
}
