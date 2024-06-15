using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("checks")]
    public class PaymentCheck
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long checkid { get; set; }
        public long paymentid { get; set; }
        public string checkno { get; set; } = string.Empty;
        public long bankid { get; set; }
        public DateTime? checkdate { get; set; }
        public string accountnumber { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public CheckStatus status { get; set; }
        public DateTime? replacedcheckdate { get; set; }
        public string replacedcheckno { get; set; } = string.Empty;
        public DateTime? datedeposited { get; set; }
        public long depositaccount { get; set; }
        public string memo { get; set; } = string.Empty;

        public Bank? bank { get; set; }
        public Payment? payment { get; set; }

    }
}
