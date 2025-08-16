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
        private readonly IRentalService _rentalService;
        private readonly IAccountService _accountService;
        private readonly ILogService _logService;

        public PaymentRepository(DataContext dataContext, IMapper mapper, IRentalService rentalService, IAccountService accountService, ILogService logService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _rentalService = rentalService;
            _accountService = accountService;
            _logService = logService;
        }

        public async Task<PaymentViewModel> Create(PaymentRequestDto requestDto)
        {
            Payment payment = _mapper.Map<Payment>(requestDto);
            payment.paymentid = 0;
            payment.status = PaymentStatus.paid;
            if (payment.transtype == Constants.PaymentTransType.creditMemo)
            {
                payment.orno = (await _dataContext.payments.Where(x => x.companyid == requestDto.companyid && x.transtype == Constants.PaymentTransType.creditMemo)
                    .MaxAsync(x => (long?)x.orno) ?? 0) + 1;
                payment.receiptno = "CN" + payment.orno.ToString();
            }
            else
                payment.receiptno = Constants.getReceiptDesc(payment.receipttype) + payment.orno.ToString();
            payment.datecreated = Utility.GetServerTime();
            payment.paymentCheck = null;
            payment.bankTransfer = null;
            if (payment.paymentmode == PaymentMode.check)
            {
                PaymentCheck paymentCheck = _mapper.Map<PaymentCheck>(requestDto.paymentCheckRequestDto);
                paymentCheck.checkid = 0;
                paymentCheck.status = CheckStatus.floating;
                paymentCheck.amount = payment.totalamount;
                payment.paymentCheck = paymentCheck;
            }
            else if (payment.paymentmode == PaymentMode.bankTransfer || payment.paymentmode == PaymentMode.eWallet)
            {
                BankTransfer bankTransfer = _mapper.Map<BankTransfer>(requestDto.bankTransfer);
                if (payment.paymentmode == PaymentMode.bankTransfer)
                {
                    var account = await _dataContext.bankAccounts.FindAsync(bankTransfer.accountid);
                    bankTransfer.bankid = account.bankid;
                }
                bankTransfer.id = 0;
                bankTransfer.amount = payment.totalamount;
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
                    detail.surveyid = payable.payableID;
                else if (payable.payableType == AppModule.Rentals)
                {
                    detail.rentalchargeid = payable.payableID;
                    var rentalCharge = await _dataContext.rentalCharges.FindAsync(detail.rentalchargeid);
                    detail.rentalid = rentalCharge.contractid;
                    detail.paymenttype = (long)rentalCharge.chargetype;
                }

                payment.paymentDetails.Add(detail);
            }
            _dataContext.payments.Add(payment);
            await _dataContext.SaveChangesAsync();

            foreach (var payable in requestDto.payables)
            {
                if (payable.payableType == AppModule.Surveying)
                {
                    var survey = await _dataContext.surveys.FindAsync(payable.payableID);
                    survey.balance = payable.balance - payable.paymentAmount;
                    if (survey.balance <= 0 && survey.status == SurveyStatus.surveyed)
                        survey.status = SurveyStatus.completed;
                }
                else if (payable.payableType == AppModule.Rentals)
                {
                    var rentalCharge = await _dataContext.rentalCharges.FindAsync(payable.payableID);
                    rentalCharge.balance = payable.balance - payable.paymentAmount;
                    await _rentalService.RecomputeContract(rentalCharge.contractid);
                }
            }

            if(payment.autocashinaccountid != null && payment.autocashinaccountid > 0)
            {
                var cashDisbursement = new CashDisbursement
                {
                    companyid = payment.companyid,
                    accountid = payment.autocashinaccountid ?? 0,
                    refdate = payment.paymentdate,
                    refno = payment.receiptno,
                    amount = payment.totalamount,
                    remarks = "Auto cash-in from payment",
                    transtype = DisbursementTransType.cashin,
                    refdisbursementid = 0,
                    status = DisbursementStatus.approved,
                    createdbyid = payment.encodedby,
                    datecreated = payment.datecreated,
                    refpaymentid = payment.paymentid
                };
                _dataContext.cashDisbursements.Add(cashDisbursement);
                await _dataContext.SaveChangesAsync();
                await _accountService.UpdateOfficeBalanceAsync(cashDisbursement.accountid, cashDisbursement.amount);
            }
            await _logService.SaveLogAsync(payment.companyid, payment.encodedby, AppModule.Payments, "New Payment", "New Payment ID: " + payment.paymentid, 0);
            return _mapper.Map<PaymentViewModel>(payment);
        }

        public async Task<ICollection<PayableViewModel>> GetPayables(long clientID, string search)
        {
            var result = await _dataContext.payables.FromSqlRaw("exec spWebReports @operation=0, @soperation=1, @clientid = {0}, @search =  {1}", clientID, search).ToListAsync();
            return _mapper.Map<ICollection<PayableViewModel>>(result);
        }

        public async Task<PaymentViewModel> GetPayment(long paymentID)
        {
            var result = await _dataContext.payments.Include(x => x.client)
                .Include(x => x.paymentDetails)
                .Include(x => x.paymentCheck).ThenInclude(x => x.bank)
                .Include(x => x.bankTransfer).ThenInclude(x => x.bank)
                .Include(x => x.createdBy).Where(x => x.paymentid == paymentID).FirstOrDefaultAsync();
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
        public async Task<ICollection<PayableViewModel>> GetPaymentDetailsAsPayables(long paymentID)
        {
            var result = await _dataContext.payables.FromSqlRaw("exec spWebReports @operation=0, @soperation=2, @paymentid = {0}", paymentID).ToListAsync();
            return _mapper.Map<ICollection<PayableViewModel>>(result);
        }

        public async Task<ICollection<PaymentViewModel>> GetPayments(int companyID, string search, DateTime startDate, DateTime endDate)
        {
            var result = await _dataContext.payments.Include(x => x.client)
                .Where(x => x.companyid == companyID && x.transtype == Constants.PaymentTransType.payment && x.paymentdate >= startDate.Date && x.paymentdate <= endDate.Date
                    && ((x.client.fname + x.client.lname).Contains(search) || x.receiptno.Contains(search)))
                .OrderByDescending(x => x.paymentdate)
                .ThenByDescending(x => x.datecreated).ToListAsync();
            return _mapper.Map<ICollection<PaymentViewModel>>(result);
        }

        public async Task<ICollection<PaymentViewModel>> GetCreditNotes(int companyID, string search, DateTime startDate, DateTime endDate)
        {
            var result = await _dataContext.payments.Include(x => x.client)
                .Where(x => x.companyid == companyID && x.transtype == Constants.PaymentTransType.creditMemo && x.paymentdate >= startDate.Date && x.paymentdate <= endDate.Date
                    && (x.client.fname + x.client.lname).Contains(search) && x.receiptno.Contains(search))
                .OrderByDescending(x => x.paymentdate)
                .ThenByDescending(x => x.datecreated).ToListAsync();
            return _mapper.Map<ICollection<PaymentViewModel>>(result);
        }

        public async Task<long> GetReceiptNo(int companyID, ReceiptType receiptType)
        {
            return (await _dataContext.payments.Where(x => x.companyid == companyID && x.transtype == Constants.PaymentTransType.payment && x.receipttype == receiptType).MaxAsync(x => (long?)x.orno) ?? 0) + 1;
        }

        public async Task<ICollection<PaymentDetailViewModel>> GetSurveyPaymentDetails(long surveyID)
        {
            var result = await _dataContext.paymentDetails.Include(x => x.payment)
                .Where(x => x.payableType == AppModule.Surveying && x.surveyid == surveyID && x.payment.status != PaymentStatus.@void).ToListAsync();
            return _mapper.Map<ICollection<PaymentDetailViewModel>>(result);
        }

        public async Task<bool> IsDuplicateReceipt(int companyID, ReceiptType receiptType, long receiptNo)
        {
            return await _dataContext.payments.AnyAsync(x => x.companyid == companyID && x.transtype == Constants.PaymentTransType.payment && x.receipttype == receiptType && x.status != PaymentStatus.@void && x.orno == receiptNo);
        }

        public async Task<bool> VoidPayment(long paymentID, long employeeid, string remarks)
        {
            var entity = await GetPaymentByID(paymentID);
            if (entity != null)
            {
                entity.status = PaymentStatus.@void;
                entity.voidremarks = remarks;
                await _dataContext.SaveChangesAsync();
                foreach (var detail in entity.paymentDetails)
                {
                    if (detail.payableType == AppModule.Surveying)
                    {
                        var survey = await _dataContext.surveys.FindAsync(detail.surveyid);
                        survey.balance += detail.amount;
                        if (survey.balance > 0 && survey.status == SurveyStatus.completed)
                            survey.status = SurveyStatus.surveyed;
                    }
                    else if (detail.payableType == AppModule.Rentals)
                        await _rentalService.RecomputeContract(detail.rentalid);
                }
                if (entity.autocashinaccountid != null && entity.autocashinaccountid > 0)
                {
                    var cashIn = await _dataContext.cashDisbursements.Where(x => x.refpaymentid == paymentID).FirstOrDefaultAsync();
                    if (cashIn != null)
                    {
                        cashIn.status = DisbursementStatus.@void;
                        await _accountService.UpdateOfficeBalanceAsync(entity.autocashinaccountid ?? 0, entity.totalamount * -1);
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
