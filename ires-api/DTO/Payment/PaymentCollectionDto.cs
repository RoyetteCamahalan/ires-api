namespace ires_api.DTO.Payment
{
    public class PaymentCollectionDto
    {
        public decimal totalCash { get; set; }
        public decimal totalCheck { get; set; }
        public decimal totalBankTransfer { get; set; }
        public decimal totalWallet { get; set; }
        public decimal totalPayment { get; set; }
        public ICollection<PaymentDto>? payments { get; set; }
    }
}
