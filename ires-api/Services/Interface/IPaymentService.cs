using ires_api.DTO;
using ires_api.DTO.Payment;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IPaymentService
    {
        public Task<ICollection<Payment>> GetPayments(int companyID, string search);
        public Task<ICollection<Payment>> GetPayments(int companyID, string search, DateTime startDate, DateTime endDate);
        public Task<Payment> GetPayment(long paymentID);
        public Task<ICollection<PaymentDetail>> GetPaymentDetails(long paymentID);
        public Task<ICollection<PaymentDetail>> GetSurveyPaymentDetails(long surveyID);
        public Task<ICollection<PayableDto>> GetPayables(long clientID, string search);
        public Task<long> GetReceiptNo(int companyID, int receiptType);
        public Task<bool> IsDuplicateReceipt(int companyID, int receiptType, long receiptNo);
        public Task<Payment> Create(PaymentRequestDto requestDto);
        public Task<bool> VoidPayment(long paymentID);
    }
}
