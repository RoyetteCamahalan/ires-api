using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("rentalcharges")]
    public class RentalCharge : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public RentalContract? rentalContract { get; set; }
        public OtherFee? otherFee { get; set; }
    }
}
