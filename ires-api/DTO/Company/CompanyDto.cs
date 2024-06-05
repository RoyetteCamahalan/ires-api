namespace ires_api.DTO.Company
{
    public class CompanyDto
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
        public string? contactno { get; set; }
        public DateTime? subscriptionexpiry { get; set; }
        public int planid { get; set; }
        public bool isexpired { get; set; }
    }
}
