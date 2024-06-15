using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("banktobank")]
    public class BankTransfer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public long paymentid { get; set; }
        public long bankid { get; set; }
        public long accountid { get; set; }
        public DateTime? paymentdate { get; set; }
        public decimal amount { get; set; }
        public string memo { get; set; } = string.Empty;
        public string refno { get; set; } = string.Empty;
        public Payment? payment { get; set; }
        public Bank? bank { get; set; }
    }
}
