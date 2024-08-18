namespace ires.Domain.DTO.CreditNote
{
    public class CreditMemoTypeRequestDto
    {
        public long id { get; set; }
        public string name { get; set; } = string.Empty;
        public bool isactive { get; set; }
    }
}
