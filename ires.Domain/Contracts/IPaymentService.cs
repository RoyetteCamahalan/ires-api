using ires.Domain.Common;
using ires.Domain.DTO.Payment;
using ires.Domain.Enumerations;

namespace ires.Domain.Contracts
{
    public interface IPaymentService
    {
        public Task<PaginatedResult<PaymentViewModel>> GetPayments(PaginationRequest request);
        public Task<PaymentViewModel> GetPayment(long paymentID);
        public Task<ICollection<PaymentDetailViewModel>> GetPaymentDetails(long paymentID);
        public Task<ICollection<PayableViewModel>> GetPaymentDetailsAsPayables(long paymentID);
        public Task<ICollection<PaymentDetailViewModel>> GetSurveyPaymentDetails(long surveyID);
        public Task<PaginatedResult<PayableViewModel>> GetPayables(long clientID, PaginationRequest request);
        public Task<long> GetReceiptNo(ReceiptType receiptType);
        public Task<bool> IsDuplicateReceipt(ReceiptType receiptType, long receiptNo);
        public Task<PaymentViewModel> Create(PaymentRequestDto requestDto);
        public Task VoidPayment(long paymentID, string remarks);


        public Task<PaginatedResult<PaymentViewModel>> GetCreditNotes(PaginationRequest request);
    }
}
