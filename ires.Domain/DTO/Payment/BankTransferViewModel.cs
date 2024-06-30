using ires.Domain.DTO.Bank;

namespace ires.Domain.DTO.Payment
{
    public class BankTransferViewModel
    {
        public long id { get; set; }
        public long paymentid { get; set; }
        public long bankid { get; set; }
        public long accountid { get; set; }
        public DateTime? paymentdate { get; set; }
        public decimal amount { get; set; }
        public string memo { get; set; } = string.Empty;
        public string refno { get; set; } = string.Empty;
        public BankViewModel? bank { get; set; }
    }
}
