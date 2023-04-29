namespace ires_api.DTO
{
    public class OfficeDto
    {
        public long accountid { get; set; }
        public int companyid { get; set; }
        public string accountname { get; set; } = string.Empty;
        public bool isactive { get; set; }
        public string memo { get; set; } = string.Empty;
    }
}
