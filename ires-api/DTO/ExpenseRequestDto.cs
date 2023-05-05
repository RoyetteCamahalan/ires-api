namespace ires_api.DTO
{
    public class ExpenseRequestDto
    {
        public long expenseid { get; set; }
        public int companyid { get; set; }
        public long accountid { get; set; }
        public long expensetypeid { get; set; }
        public string refno { get; set; } = string.Empty;
        public DateTime? refdate { get; set; }
        public decimal amount { get; set; }
        public string memo { get; set; } = string.Empty;
        public long payeeid { get; set; }
        public bool usepettycash { get; set; }
        public long createdbyid { get; set; }
        public long updatedbyid { get; set; }
    }
}
