using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("payment")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long paymentid { get; set; }
        public int companyid { get; set; }
        public DateTime? paymentdate { get; set; }
        public long custid { get; set; }
        public long encodedby { get; set; }
        public long orno { get; set; }
        public ReceiptType receipttype { get; set; }
        public string receiptno { get; set; } = string.Empty;
        public PaymentMode paymentmode { get; set; } //See Constants.PaymentMode
        public decimal totalamount { get; set; }
        public decimal tender { get; set; }
        public decimal change { get; set; }
        public PaymentStatus status { get; set; }
        public DateTime? daterefunded { get; set; }
        public long replacementpaymentid { get; set; }
        public string replacedreceipts { get; set; } = string.Empty;
        public string transtype { get; set; } = string.Empty; //See Constants.PaymentTransType
        public string paidby { get; set; } = string.Empty;
        public string remarks { get; set; } = string.Empty;
        public DateTime? datecreated { get; set; }
        public long? creditmemotypeid { get; set; }

        public long? autocashinaccountid { get; set; }

        public Client? client { get; set; }
        public PaymentCheck? paymentCheck { get; set; }
        public BankTransfer? bankTransfer { get; set; }
        public List<PaymentDetail> paymentDetails { get; set; } = new List<PaymentDetail>();
        public Employee? createdBy { get; set; }
        public CreditMemoType? creditMemoType { get; set; }
        public string voidremarks { get; set; } = string.Empty;

    }
}
