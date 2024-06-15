namespace ires.Domain.DTO.Payment
{
    public class BankTransferRequestDto
    {
        public long bankid { get; set; } = 0;
        public long accountid { get; set; } = 0;
        public DateTime? paymentdate { get; set; }
        public decimal amount { get; set; } = 0;
        public string memo { get; set; } = string.Empty;
        public string refno { get; set; } = string.Empty;
    }
}
