namespace ires.Domain.DTO.ExpenseType
{
    public class ExpenseTypeRequestDto
    {
        public long expensetypeid { get; set; }
        public int companyid { get; set; }
        public string expensetypedesc { get; set; } = string.Empty;
        public int expensetypecat { get; set; }
        public bool isactive { get; set; }
        public long createdbyid { get; set; }
        public long updatedbyid { get; set; }
    }
}
