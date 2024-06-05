namespace ires_api.DTO.ExpenseType
{
    public class ExpenseTypeCategoryDto
    {
        public int expensecatid { get; set; }
        public string description { get; set; } = string.Empty;
        public bool isactive { get; set; }
    }
}
