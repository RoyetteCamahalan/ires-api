using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires_api.Models
{
    [Table("expenses")]
    public class Expense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long expenseid { get; set; }
        public int companyid { get; set; }
        public long transno { get; set; }
        public long accountid { get; set; }

        [Column("expensetype")]
        public long expensetypeid { get; set; }
        public string refno { get; set; } = string.Empty;
        public DateTime? refdate { get; set; }
        public decimal amount { get; set; }
        public string memo { get; set; } = string.Empty;
        public DateTime? transdate { get; set; }
        public int status { get; set; }
        public long payeeid { get; set; }
        public decimal balance { get; set; } = 0;
        public decimal runningbalance { get; set; } = 0;
        public bool usepettycash { get; set; }
        public long createdbyid { get; set; }
        public long updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }

        public Office? office { get; set; }
        public ExpenseType? expenseType { get; set; }
        public Vendor? vendor { get; set; }
    }
}
