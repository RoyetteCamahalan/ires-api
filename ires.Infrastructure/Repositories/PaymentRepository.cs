using AutoMapper;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO.Payment;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ISurveyService _surveyService;
        private readonly ILogService _logService;

        public PaymentRepository(DataContext dataContext, IMapper mapper, ISurveyService surveyService, ILogService logService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _surveyService = surveyService;
            _logService = logService;
        }

        public async Task<PaymentViewModel> Create(PaymentRequestDto requestDto)
        {
            Payment payment = _mapper.Map<Payment>(requestDto);
            payment.paymentid = 0;
            payment.status = PaymentStatus.paid;
            payment.receiptno = Constants.getReceiptDesc(payment.receipttype) + payment.orno.ToString();
            payment.datecreated = DateTime.Now;
            payment.paymentCheck = null;
            payment.bankTransfer = null;
            if (payment.paymentmode == PaymentMode.check)
            {
                PaymentCheck paymentCheck = _mapper.Map<PaymentCheck>(requestDto.paymentCheckRequestDto);
                paymentCheck.checkid = 0;
                paymentCheck.status = CheckStatus.floating;
                payment.paymentCheck = paymentCheck;
            }
            else if (payment.paymentmode == PaymentMode.bankTransfer || payment.paymentmode == PaymentMode.eWallet)
            {
                BankTransfer bankTransfer = _mapper.Map<BankTransfer>(requestDto.bankTransfer);
                bankTransfer.id = 0;
                payment.bankTransfer = bankTransfer;
            }
            foreach (var payable in requestDto.payables)
            {
                PaymentDetail detail = new()
                {
                    balance = payable.balance,
                    amount = payable.paymentAmount,
                    payableType = payable.payableType,
                    paymenttype = 0,
                    runningbalance = payable.balance - payable.paymentAmount
                };
                if (payable.payableType == AppModule.Surveying)
                {
                    detail.surveyid = payable.payableID;
                    var survey = await _surveyService.GetByID(detail.surveyid);
                    survey.balance = payable.balance - payable.paymentAmount;
                    if (survey.balance <= 0 && survey.status == SurveyStatus.surveyed)
                        survey.status = SurveyStatus.completed;
                }

                payment.paymentDetails.Add(detail);
            }
            _dataContext.payments.Add(payment);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<PaymentViewModel>(payment);
        }

        public async Task<ICollection<PayableViewModel>> GetPayables(long clientID, string search)
        {
            return await _dataContext.surveys.Where(x => x.custid == clientID && x.balance > 0 && ("Survey Fee - " + x.propertyname).Contains(search)).Select(x => new PayableViewModel
            {
                payableType = AppModule.Surveying,
                payableID = x.id,
                description = "Survey Fee - " + x.propertyname + " (" + (x.surveydate ?? DateTime.Now).ToString(Constants.dateFormat) + ")",
                grossAmount = x.contractprice,
                balance = x.balance
            }).ToListAsync();
        }

        public async Task<PaymentViewModel> GetPayment(long paymentID)
        {
            var result = await GetPaymentByID(paymentID);
            return _mapper.Map<PaymentViewModel>(result);
        }

        private async Task<Payment> GetPaymentByID(long paymentID)
        {
            return await _dataContext.payments.Include(x => x.client)
                .Include(x => x.paymentDetails)
                .Include(x => x.createdBy).Where(x => x.paymentid == paymentID).FirstOrDefaultAsync();
        }

        public async Task<ICollection<PaymentDetailViewModel>> GetPaymentDetails(long paymentID)
        {
            var result = await _dataContext.paymentDetails.Where(x => x.paymentid == paymentID).ToListAsync();
            return _mapper.Map<ICollection<PaymentDetailViewModel>>(result);
        }

        public async Task<ICollection<PaymentViewModel>> GetPayments(int companyID, string search)
        {
            var result = await _dataContext.payments.Include(x => x.client)
                .Where(x => x.companyid == companyID && (x.client.fname + x.client.lname).Contains(search) && x.receiptno.Contains(search))
                .OrderByDescending(x => x.paymentdate)
                .ThenByDescending(x => x.datecreated).ToListAsync();
            return _mapper.Map<ICollection<PaymentViewModel>>(result);
        }

        public async Task<ICollection<PaymentViewModel>> GetPayments(int companyID, string search, DateTime startDate, DateTime endDate)
        {
            var result = await _dataContext.payments.Include(x => x.client)
                .Where(x => x.companyid == companyID && x.paymentdate >= startDate.Date && x.paymentdate <= endDate.Date
                    && (x.client.fname + x.client.lname).Contains(search) && x.receiptno.Contains(search))
                .OrderByDescending(x => x.paymentdate)
                .ThenByDescending(x => x.datecreated).ToListAsync();
            return _mapper.Map<ICollection<PaymentViewModel>>(result);
        }

        public async Task<long> GetReceiptNo(int companyID, ReceiptType receiptType)
        {
            return (await _dataContext.payments.Where(x => x.companyid == companyID && x.receipttype == receiptType).MaxAsync(x => (long?)x.orno) ?? 0) + 1;
        }

        public async Task<ICollection<PaymentDetailViewModel>> GetSurveyPaymentDetails(long surveyID)
        {
            var result = await _dataContext.paymentDetails.Include(x => x.payment)
                .Where(x => x.payableType == AppModule.Surveying && x.surveyid == surveyID && x.payment.status != PaymentStatus.@void).ToListAsync();
            return _mapper.Map<ICollection<PaymentDetailViewModel>>(result);
        }

        public async Task<bool> IsDuplicateReceipt(int companyID, ReceiptType receiptType, long receiptNo)
        {
            return await _dataContext.payments.AnyAsync(x => x.companyid == companyID && x.receipttype == receiptType && x.status != PaymentStatus.@void && x.orno == receiptNo);
        }

        public async Task<bool> VoidPayment(long paymentID, long employeeid)
        {
            var entity = await GetPaymentByID(paymentID);
            if (entity != null)
            {
                entity.status = PaymentStatus.@void;
                foreach (var detail in entity.paymentDetails)
                {
                    if (detail.payableType == AppModule.Surveying)
                    {
                        var survey = await _surveyService.GetByID(detail.surveyid);
                        survey.balance += detail.amount;
                        if (survey.balance > 0 && survey.status == SurveyStatus.completed)
                            survey.status = SurveyStatus.surveyed;
                    }
                }
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(entity.companyid, employeeid, AppModule.Payments, "Void Payment", "Void Payment ID: " + paymentID, 0);
                return true;
            }
            return false;
        }
    }
}
