namespace ires.Domain.DTO.Expense
{
    public class ExpenseReportViewModel
    {
        public decimal totalExpense { get; set; }
        public decimal totalOperatingExpense { get; set; }
        public decimal totalNonOperatingExpense { get; set; }
        public IEnumerable<ExpenseViewModel>? expenses { get; set; }
    }
}
