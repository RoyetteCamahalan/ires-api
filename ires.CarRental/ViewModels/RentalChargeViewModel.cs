using ires.Domain.DTO.OtherFee;
using ires.Domain.Enumerations;

namespace ires.Application.ViewModels
{
    public class RentalChargeViewModel
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
        public RentalContractViewModel? rentalContract { get; set; }
        public OtherFeeViewModel? otherFee { get; set; }
    }
}
