namespace ires_api.DTO
{
    public class BankDto
    {
        public long bankid { get; set; }
        public int companyid { get; set; }
        public string name { get; set; } = string.Empty;
        public bool isewallet { get; set; }
    }
}
