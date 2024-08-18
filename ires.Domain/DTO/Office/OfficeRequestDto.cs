namespace ires.Domain.DTO.Office
{
    public class OfficeRequestDto
    {
        public long accountid { get; set; }
        public int companyid { get; set; }
        public string accountname { get; set; } = string.Empty;
        public bool isactive { get; set; }
        public string memo { get; set; } = string.Empty;
    }
}
