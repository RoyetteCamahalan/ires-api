using ires.Domain.Enumerations;

namespace ires.Domain.DTO.Company
{
    public class RegisterCompanyRequestDto
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string contactno { get; set; } = string.Empty;
        public string adminfirstname { get; set; } = string.Empty;
        public string adminlastname { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public BillingCycle billingcycle { get; set; }
        public int planid { get; set; }
    }
}
