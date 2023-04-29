using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IPaymentService
    {
        public ICollection<Payment> GetPayments(int companyID, string search);
        public ICollection<Payment> GetPayments(int companyID, string search, DateTime startDate, DateTime endDate);
        public Payment GetPayment(long paymentID);
        public ICollection<PaymentDetail> GetPaymentDetails(long paymentID);
        public ICollection<PaymentDetail> GetSurveyPaymentDetails(long surveyID);
        public ICollection<PayableDto> GetPayables(long clientID, string search);
        public long GetReceiptNo(int companyID, int receiptType);
        public bool IsDuplicateReceipt(int companyID, int receiptType, long receiptNo);
        public Payment Create(PaymentRequestDto requestDto);
        public bool VoidPayment(long paymentID);
    }
}
