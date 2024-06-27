using ires.Domain.DTO.Payment;
using ires.Domain.Enumerations;

namespace ires.Domain.Contracts
{
    public interface IPaymentService
    {
        public Task<ICollection<PaymentViewModel>> GetPayments(int companyID, string search, DateTime startDate, DateTime endDate);
        public Task<PaymentViewModel> GetPayment(long paymentID);
        public Task<ICollection<PaymentDetailViewModel>> GetPaymentDetails(long paymentID);
        public Task<ICollection<PayableViewModel>> GetPaymentDetailsAsPayables(long paymentID);
        public Task<ICollection<PaymentDetailViewModel>> GetSurveyPaymentDetails(long surveyID);
        public Task<ICollection<PayableViewModel>> GetPayables(long clientID, string search);
        public Task<long> GetReceiptNo(int companyID, ReceiptType receiptType);
        public Task<bool> IsDuplicateReceipt(int companyID, ReceiptType receiptType, long receiptNo);
        public Task<PaymentViewModel> Create(PaymentRequestDto requestDto);
        public Task<bool> VoidPayment(long paymentID, long employeeid, string remarks);


        public Task<ICollection<PaymentViewModel>> GetCreditNotes(int companyID, string search, DateTime startDate, DateTime endDate);
    }
}
