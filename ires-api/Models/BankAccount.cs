using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires_api.Models
{
    [Table("bankaccounts")]
    public class BankAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long accountid { get; set; }
        public int companyid { get; set; }
        public string accountname { get; set; } = string.Empty;
        public string accountno { get; set; } = string.Empty;
        public long bankid { get; set; }
        public string bankpreferredbranch { get; set; } = string.Empty;
        public bool isactive { get; set; }
        public long createdbyid { get; set; }
        public DateTime? datecreated { get; set; }

        public Bank? bank { get; set; }

    }
}
