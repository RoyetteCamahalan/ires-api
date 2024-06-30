using ires.Domain.DTO.Bank;
using ires.Domain.Enumerations;

namespace ires.Domain.DTO.Payment
{
    public class PaymentCheckViewModel
    {
        public long checkid { get; set; }
        public long paymentid { get; set; }
        public string checkno { get; set; } = string.Empty;
        public long bankid { get; set; }
        public DateTime? checkdate { get; set; }
        public string accountnumber { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public CheckStatus status { get; set; }
        public DateTime? replacedcheckdate { get; set; }
        public string replacedcheckno { get; set; } = string.Empty;
        public DateTime? datedeposited { get; set; }
        public long depositaccount { get; set; }
        public string memo { get; set; } = string.Empty;

        public BankViewModel? bank { get; set; }
    }
}
