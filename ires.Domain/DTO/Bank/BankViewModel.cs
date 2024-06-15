namespace ires.Domain.DTO.Bank
{
    public class BankViewModel
    {
        public long bankid { get; set; }
        public int companyid { get; set; }
        public string name { get; set; } = string.Empty;
        public bool isewallet { get; set; }
    }
}
