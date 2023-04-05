namespace ires_api.DTO
{
    public class BankAccountDto
    {
        public long accountid { get; set; }
        public int companyid { get; set; }
        public string accountname { get; set; } = string.Empty;
        public string accountno { get; set; } = string.Empty;
        public long bankid { get; set; }
        public string bankpreferredbranch { get; set; } = string.Empty;
        public bool isactive { get; set; }

        public BankDto? bank { get; set; }
    }
}
