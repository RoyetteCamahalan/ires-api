using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("othercharges")]
    public class OtherCharge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long chargeid { get; set; }
        public long surveyid { get; set; }
        public long invoiceno { get; set; }
        public long lotid { get; set; }
        public DateTime chargedate { get; set; }
        public decimal chargeamount { get; set; }
        public decimal interestamount { get; set; }
        public decimal balance { get; set; }
        public long chargetype { get; set; }
        public decimal runningbalance { get; set; }
        public int interestype { get; set; } //See Constants.InterestType
        public decimal interestpercentage { get; set; }
        public Survey? survey { get; set; }
        public OtherFee? fee { get; set; }
    }	
}
