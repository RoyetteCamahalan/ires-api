using ires_api.DTO.Payment;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IPaymentService
    {
        public Task<ICollection<PaymentViewModel>> GetPayments(int companyID, string search);
        public Task<ICollection<PaymentViewModel>> GetPayments(int companyID, string search, DateTime startDate, DateTime endDate);
        public Task<PaymentViewModel> GetPayment(long paymentID);
        public Task<ICollection<PaymentDetail>> GetPaymentDetails(long paymentID);
        public Task<ICollection<PaymentDetail>> GetSurveyPaymentDetails(long surveyID);
        public Task<ICollection<PayableViewModel>> GetPayables(long clientID, string search);
        public Task<long> GetReceiptNo(int companyID, int receiptType);
        public Task<bool> IsDuplicateReceipt(int companyID, int receiptType, long receiptNo);
        public Task<PaymentViewModel> Create(PaymentRequestDto requestDto);
        public Task<bool> VoidPayment(long paymentID);
    }
}
