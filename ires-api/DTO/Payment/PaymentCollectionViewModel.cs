namespace ires_api.DTO.Payment
{
    public class PaymentCollectionViewModel
    {
        public decimal totalCash { get; set; }
        public decimal totalCheck { get; set; }
        public decimal totalBankTransfer { get; set; }
        public decimal totalWallet { get; set; }
        public decimal totalPayment { get; set; }
        public ICollection<PaymentViewModel>? payments { get; set; }
    }
}
