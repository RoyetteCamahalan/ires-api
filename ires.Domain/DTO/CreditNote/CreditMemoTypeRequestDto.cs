namespace ires.Domain.DTO.CreditNote
{
    public class CreditMemoTypeRequestDto
    {
        public long id { get; set; }
        public int companyid { get; set; }
        public string name { get; set; } = string.Empty;
        public bool isactive { get; set; }
        public long createdbyid { get; set; }
        public long updatedbyid { get; set; }
    }
}
