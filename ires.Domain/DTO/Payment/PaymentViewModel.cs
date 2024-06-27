using ires.Domain.DTO.Client;
using ires.Domain.DTO.CreditNote;
using ires.Domain.DTO.Employee;
using ires.Domain.Enumerations;

namespace ires.Domain.DTO.Payment
{
    public class PaymentViewModel
    {
        public long paymentid { get; set; }
        public int companyid { get; set; }
        public DateTime? paymentdate { get; set; }
        public long custid { get; set; }
        public long encodedby { get; set; }
        public long orno { get; set; }
        public ReceiptType receipttype { get; set; } //See Constants.ReceiptType
        public string receiptno { get; set; } = string.Empty;
        public PaymentMode paymentmode { get; set; }
        public decimal totalamount { get; set; }
        public PaymentStatus status { get; set; }
        public string transtype { get; set; } = string.Empty; //See Constants.PaymentTransType
        public string paidby { get; set; } = string.Empty;
        public string remarks { get; set; } = string.Empty;
        public decimal payablebalance { get; set; }
        public long? creditmemotypeid { get; set; }
        public ClientViewModel? client { get; set; }
        public ICollection<PayableViewModel>? payables { get; set; }
        public EmployeeViewModel? createdBy { get; set; }
        public List<PaymentDetailViewModel> paymentDetails { get; set; } = new List<PaymentDetailViewModel>();
        public CreditMemoTypeViewModel? creditMemoType { get; set; }
        public string voidremarks { get; set; } = string.Empty;
    }
}
