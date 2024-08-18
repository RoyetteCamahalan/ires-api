using ires.Domain.Enumerations;

namespace ires.Domain.DTO.Payment
{
    public class PaymentRequestDto
    {
        public long paymentid { get; set; }
        public DateTime? paymentdate { get; set; }
        public long custid { get; set; }
        public long orno { get; set; }
        public ReceiptType receipttype { get; set; }
        public int paymentmode { get; set; } //See Constants.PaymentMode
        public decimal totalamount { get; set; }
        public string transtype { get; set; } = string.Empty; //See Constants.PaymentTransType
        public string paidby { get; set; } = string.Empty;
        public string remarks { get; set; } = string.Empty;
        public long? creditmemotypeid { get; set; }
        public string voidremarks { get; set; } = string.Empty;
        public List<PayableRequestDto> payables { get; set; } = [];
        public PaymentCheckRequestDto? paymentCheckRequestDto { get; set; }
        public BankTransferRequestDto? bankTransfer { get; set; }
    }
}
