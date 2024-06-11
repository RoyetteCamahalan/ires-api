using ires_api.DTO.Client;
using ires_api.DTO.Employee;

namespace ires_api.DTO.Payment
{
    public class PaymentViewModel
    {
        public long paymentid { get; set; }
        public int companyid { get; set; }
        public DateTime? paymentdate { get; set; }
        public long custid { get; set; }
        public long encodedby { get; set; }
        public long orno { get; set; }
        public int receipttype { get; set; } //See Constants.ReceiptType
        public string receiptno { get; set; } = string.Empty;
        public int paymentmode { get; set; } //See Constants.PaymentMode
        public decimal totalamount { get; set; }
        public int status { get; set; } //See Constants.PaymentStatus
        public string transtype { get; set; } = string.Empty; //See Constants.PaymentTransType
        public string paidby { get; set; } = string.Empty;
        public string remarks { get; set; } = string.Empty;
        public decimal payablebalance { get; set; }
        public ClientViewModel? client { get; set; }
        public List<PayableViewModel>? payables { get; set; }
        public EmployeeViewModel? createdBy { get; set; }
        public List<PaymentDetailViewModel> paymentDetails { get; set; } = new List<PaymentDetailViewModel>();
    }
}
