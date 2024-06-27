namespace ires.Domain.DTO.Company
{
    public class UpdateCompanyRequestDto
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string contactno { get; set; } = string.Empty;
        public long updatedbyid { get; set; }
    }
}
