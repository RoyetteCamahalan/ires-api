namespace ires_api.DTO.ExpenseType
{
    public class ExpenseTypeDto
    {
        public long expensetypeid { get; set; }
        public int companyid { get; set; }
        public string expensetypedesc { get; set; } = string.Empty;
        public int expensetypecat { get; set; }
        public bool isactive { get; set; }
        public ExpenseTypeCategoryDto? category { get; set; }
    }
}
