namespace ires_api.DTO
{
    public class ExpenseDto
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
        public int status { get; set; }
        public long payeeid { get; set; }
        public bool usepettycash { get; set; }

        public OfficeDto? office { get; set; }
        public ExpenseTypeDto? expenseType { get; set; }
        public VendorDto? vendor { get; set; }
    }
}
