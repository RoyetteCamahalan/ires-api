namespace ires.Domain.DTO.Company
{
    public class CompanyViewModel
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
        public string? contactno { get; set; }
        public string email { get; set; } = string.Empty;
        public DateTime? subscriptionexpiry { get; set; }
        public decimal amount { get; set; }
        public int planid { get; set; }
        public bool isexpired { get => subscriptionexpiry < Utility.GetServerTime().AddDays(amount == 0 ? -1 : -(Constants.BillExtension + 1)); }
        public bool isverified { get; set; }
        public int apptour { get; set; }
        public string logo { get; set; } = string.Empty;
    }
}
