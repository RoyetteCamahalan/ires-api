using ires.Domain.DTO.ExpenseType;
using ires.Domain.DTO.Vendor;
using ires.Domain.Enumerations;

namespace ires.Domain.DTO.AccountPayable
{
    public class AccountPayableViewModel
    {
        public long chargeid { get; set; }
        public int companyid { get; set; }
        public long vendorid { get; set; }
        public DateTime? dateposted { get; set; }
        public DateTime? actualdate { get; set; }
        public long expensetypeid { get; set; }
        public string refno { get; set; } = string.Empty;
        public string memo { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public AccountPayableStatus status { get; set; }
        public decimal balance { get; set; }
        public decimal runningbalance { get; set; }
        public long createdbyid { get; set; }
        public DateTime? datecreated { get; set; }
        public long updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }

        public VendorViewModel? vendor { get; set; }
        public ExpenseTypeViewModel? expenseType { get; set; }
    }
}
