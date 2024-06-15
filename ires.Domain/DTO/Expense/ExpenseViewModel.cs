using ires.Domain.DTO.ExpenseType;
using ires.Domain.DTO.Office;
using ires.Domain.DTO.Vendor;
using ires.Domain.Enumerations;

namespace ires.Domain.DTO.Expense
{
    public class ExpenseViewModel
    {
        public long expenseid { get; set; }
        public int companyid { get; set; }
        public long transno { get; set; }
        public long accountid { get; set; }
        public long expensetypeid { get; set; }
        public string refno { get; set; } = string.Empty;
        public DateTime? refdate { get; set; }
        public decimal amount { get; set; }
        public string memo { get; set; } = string.Empty;
        public DateTime? transdate { get; set; }
        public ExpenseStatus status { get; set; }
        public long payeeid { get; set; }
        public bool usepettycash { get; set; }

        public OfficeViewModel? office { get; set; }
        public ExpenseTypeViewModel? expenseType { get; set; }
        public VendorViewModel? vendor { get; set; }
    }
}
