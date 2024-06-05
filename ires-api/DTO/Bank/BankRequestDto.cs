namespace ires_api.DTO.Bank
{
    public class BankRequestDto
    {
        public long bankid { get; set; }
        public int companyid { get; set; }
        public string name { get; set; } = string.Empty;
        public bool isewallet { get; set; }
    }
}
