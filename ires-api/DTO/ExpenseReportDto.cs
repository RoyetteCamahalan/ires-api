namespace ires_api.DTO
{
    public class ExpenseReportDto
    {
        public decimal totalExpense { get; set; }
        public decimal totalOperatingExpense { get; set; }
        public decimal totalNonOperatingExpense { get; set; }
        public ICollection<ExpenseDto>? expenses { get; set; }
    }
}
