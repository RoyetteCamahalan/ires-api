using AutoMapper;
using ires_api.Data;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
{
    public class PaymentRepository : IPaymentService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ISurveyService _surveyService;

        public PaymentRepository(DataContext dataContext, IMapper mapper, ISurveyService surveyService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _surveyService = surveyService;
        }

        public Payment Create(PaymentRequestDto requestDto)
        {
            Payment payment = _mapper.Map<Payment>(requestDto);
            payment.paymentid = 0;
            payment.status = Constants.PaymentStatus.paid;
            payment.receiptno = Constants.ReceiptType.getReceiptDesc(payment.receipttype) + payment.orno.ToString();
            payment.datecreated = DateTime.Now;
            payment.paymentCheck = null;
            payment.bankTransfer = null;
            if (payment.paymentmode == Constants.PaymentMode.check)
            {
                PaymentCheck paymentCheck = _mapper.Map<PaymentCheck>(requestDto.paymentCheckRequestDto);
                paymentCheck.checkid = 0;
                payment.status = Constants.CheckStatus.floating;
                payment.paymentCheck = paymentCheck;
            }
            else if (payment.paymentmode == Constants.PaymentMode.bankTransfer || payment.paymentmode == Constants.PaymentMode.eWallet)
            {
                BankTransfer bankTransfer = _mapper.Map<BankTransfer>(requestDto.bankTransfer);
                bankTransfer.id = 0;
                payment.bankTransfer = bankTransfer;
            }
            foreach (var payable in requestDto.payables)
            {
                PaymentDetail detail = new PaymentDetail
                {
                    balance = payable.balance,
                    amount = payable.paymentAmount,
                    payableType = payable.payableType,
                    paymenttype = 0,
                    runningbalance = payable.balance - payable.paymentAmount
                };
                if (payable.payableType == Constants.AppModules.survey)
                {
                    detail.surveyid = payable.payableID;
                    Survey survey = _surveyService.GetSurveyByID(detail.surveyid);
                    survey.balance = payable.balance - payable.paymentAmount;
                    if (survey.balance <= 0 && survey.status == Constants.SurveyStatus.surveyed)
                        survey.status = Constants.SurveyStatus.completed;
                }

                payment.paymentDetails.Add(detail);
            }
            _dataContext.payments.Add(payment);
            _dataContext.SaveChanges();
            return payment;
        }

        public ICollection<PayableDto> GetPayables(long clientID, string search)
        {
            List<PayableDto> payables = new List<PayableDto>();
            payables = _dataContext.surveys.Where(x => x.custid == clientID && x.balance > 0 && ("Survey Fee - " + x.propertyname).Contains(search)).Select(x => new PayableDto
            {
                payableType = Constants.AppModules.survey,
                payableID = x.id,
                description = "Survey Fee - " + x.propertyname + " (" + (x.surveydate ?? DateTime.Now).ToString(Constants.dateFormat) + ")",
                grossAmount = x.contractprice,
                balance = x.balance
            }).ToList();

            return payables;
        }

        public Payment GetPayment(long paymentID)
        {
            return _dataContext.payments.Include(x => x.client)
                .Include(x => x.paymentDetails)
                .Include(x => x.createdBy).Where(x => x.paymentid == paymentID).FirstOrDefault();
        }

        public ICollection<PaymentDetail> GetPaymentDetails(long paymentID)
        {
            throw new NotImplementedException();
        }

        public ICollection<Payment> GetPayments(int companyID, string search)
        {
            return _dataContext.payments.Include(x => x.client)
                .Where(x => x.companyid == companyID && (x.client.fname + x.client.lname).Contains(search) && x.receiptno.Contains(search))
                .OrderByDescending(x => x.paymentdate)
                .ThenByDescending(x => x.datecreated).ToList();
        }

        public ICollection<Payment> GetPayments(int companyID, string search, DateTime startDate, DateTime endDate)
        {
            return _dataContext.payments.Include(x => x.client)
                .Where(x => x.companyid == companyID && x.paymentdate >= startDate && x.paymentdate <= endDate
                    && (x.client.fname + x.client.lname).Contains(search) && x.receiptno.Contains(search))
                .OrderByDescending(x => x.paymentdate)
                .ThenByDescending(x => x.datecreated).ToList();
        }

        public long GetReceiptNo(int companyID, int receiptType)
        {
            return (_dataContext.payments.Where(x => x.companyid == companyID && x.receipttype == receiptType).Max(x => (long?)x.orno) ?? 0) + 1;
        }

        public ICollection<PaymentDetail> GetSurveyPaymentDetails(long surveyID)
        {
            return _dataContext.paymentDetails.Include(x => x.payment)
                .Where(x => x.payableType == Constants.AppModules.survey && x.surveyid == surveyID && x.payment.status != Constants.PaymentStatus.@void).ToList();
        }

        public bool IsDuplicateReceipt(int companyID, int receiptType, long receiptNo)
        {
            return _dataContext.payments.Where(x => x.companyid == companyID && x.receipttype == receiptType && x.status != Constants.PaymentStatus.@void && x.orno == receiptNo).Count() > 0;
        }

        public bool VoidPayment(long paymentID)
        {
            var payment = GetPayment(paymentID);
            if (payment != null)
            {
                payment.status = Constants.PaymentStatus.@void;
                foreach (var detail in payment.paymentDetails)
                {
                    if (detail.payableType == Constants.AppModules.survey)
                    {
                        Survey survey = _surveyService.GetSurveyByID(detail.surveyid);
                        survey.balance += detail.amount;
                        if (survey.balance > 0 && survey.status == Constants.SurveyStatus.completed)
                            survey.status = Constants.SurveyStatus.surveyed;
                    }
                }
                _dataContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
