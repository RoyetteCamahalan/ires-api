using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    }
}
