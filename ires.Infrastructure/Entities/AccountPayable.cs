using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("expenseposting")]
    public class AccountPayable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long chargeid { get; set; }
        public int companyid { get; set; }
        public long vendorid { get; set; }
        public long accountid { get; set; }
        public DateTime? dateposted { get; set; }
        public DateTime? actualdate { get; set; }
        public long expensetypeid { get; set; }
        public string refno { get; set; } = string.Empty;
        public string memo { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public int status { get; set; }
        public decimal balance { get; set; }
        public decimal runningbalance { get; set; }
        public long createdbyid { get; set; }
        public DateTime? datecreated { get; set; }
        public long updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }

        public Vendor? vendor { get; set; }
        public ExpenseType? expenseType { get; set; }
    }
}
