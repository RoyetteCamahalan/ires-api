namespace ires.Domain.DTO.BankAccount
{
    public class BankAccountRequestDto
    {
        public long accountid { get; set; }
        public string accountname { get; set; } = string.Empty;
        public string accountno { get; set; } = string.Empty;
        public long bankid { get; set; }
        public string bankpreferredbranch { get; set; } = string.Empty;
        public bool isactive { get; set; }
    }
}
