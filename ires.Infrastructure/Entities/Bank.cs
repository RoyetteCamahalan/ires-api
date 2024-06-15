using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("bank")]
    public class Bank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long bankid { get; set; }
        public int companyid { get; set; }
        public string name { get; set; } = string.Empty;
        public bool isewallet { get; set; }

        public ICollection<PaymentCheck>? checks { get; set; }
        public ICollection<BankTransfer>? bankTransfers { get; set; }
        public ICollection<BankAccount>? bankAccounts { get; set; }
    }
}
