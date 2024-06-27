namespace ires.Domain.DTO.CreditNote
{
    public class CreditMemoTypeViewModel
    {
        public long id { get; set; }
        public int companyid { get; set; }
        public string name { get; set; } = string.Empty;
        public bool isactive { get; set; }
    }
}
