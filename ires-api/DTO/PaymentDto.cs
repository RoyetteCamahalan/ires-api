namespace ires_api.DTO
{
    public class PaymentDto
    {
        public long paymentid { get; set; }
        public int companyid { get; set; }
        public DateTime? paymentdate { get; set; }
        public long custid { get; set; }
        public long encodedby { get; set; }
        public long orno { get; set; }
        public int receipttype { get; set; } //See Constants.ReceiptType
        public int paymentmode { get; set; } //See Constants.PaymentMode
        public decimal totalamount { get; set; }
        public int status { get; set; } //See Constants.PaymentStatus
        public string transtype { get; set; } = string.Empty; //See Constants.PaymentTransType
        public string paidby { get; set; } = string.Empty;
        public string remarks { get; set; } = string.Empty;
        public ClientDto? client { get; set; }
        public List<PayableDto>? payables { get; set; }
        public EmployeeDto? createdBy { get; set; }
    }
}
