namespace ires_api.DTO
{
    public class AccountPayableRequestDto
    {
        public long chargeid { get; set; }
        public int companyid { get; set; }
        public long vendorid { get; set; }
        public DateTime? actualdate { get; set; }
        public long expensetypeid { get; set; }
        public string refno { get; set; } = string.Empty;
        public string memo { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public long createdbyid { get; set; }
        public long updatedbyid { get; set; }
    }
}
