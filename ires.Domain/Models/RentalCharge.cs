using ires.Domain.Enumerations;

namespace ires.Domain.Models
{
    public class RentalCharge
    {
        public long chargeid { get; set; }
        public long contractid { get; set; }
        public long? otherfeeid { get; set; }
        public DateTime chargedate { get; set; }
        public decimal chargeamount { get; set; }
        public decimal interestamount { get; set; }
        public decimal balance { get; set; }
        public ChargeType chargetype { get; set; }
        public decimal? runningbalance { get; set; }
        public int interestype { get; set; }
        public decimal interestpercentage { get; set; }
        public decimal ewt { get; set; }
        public bool isnotified { get; set; }
        public RentalContract? rentalContract { get; set; }
        public OtherFee? otherFee { get; set; }
    }
}
