using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires_api.Models
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
        public int receipttype { get; set; } //See Constants.ReceiptType
        public string receiptno { get; set; } = string.Empty;
        public int paymentmode { get; set; } //See Constants.PaymentMode
        public decimal totalamount { get; set; }
        public decimal tender { get; set; }
        public decimal change { get; set; }
        public int status { get; set; } //See Constants.PaymentStatus
        public DateTime? daterefunded { get; set; }
        public long replacementpaymentid { get; set; }
        public string replacedreceipts { get; set; } = string.Empty;
        public string transtype { get; set; } = string.Empty; //See Constants.PaymentTransType
        public string paidby { get; set; } = string.Empty;
        public string remarks { get; set; } = string.Empty;
        public DateTime? datecreated { get; set; }

        public Client? client { get; set; }
        public PaymentCheck? paymentCheck { get; set; }
        public BankTransfer? bankTransfer { get; set; }
        public List<PaymentDetail> paymentDetails { get; set; } = new List<PaymentDetail>();
        public Employee? createdBy { get; set; }

    }
}
