namespace ires.Domain.DTO.ExpenseType
{
    public class ExpenseTypeCategoryViewModel
    {
        public int expensecatid { get; set; }
        public string description { get; set; } = string.Empty;
        public bool isactive { get; set; }
    }
}
