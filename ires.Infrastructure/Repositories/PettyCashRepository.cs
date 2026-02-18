using AutoMapper;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO.CashDisbursement;
using ires.Domain.DTO.PettyCash;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace ires.Infrastructure.Repositories
{
    public class PettyCashRepository : IPettyCashService
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public PettyCashRepository(DataContext dataContext, IAccountService accountService, IMapper mapper, ILogService logService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _mapper = mapper;
            _logService = logService;
        }
        public async Task<CashDisbursementViewModel> Create(CashDisbursementRequestDto requestDto)
        {
            var cashDisbursement = _mapper.Map<CashDisbursement>(requestDto);
            cashDisbursement.disbursementid = 0;
            cashDisbursement.datecreated = Utility.GetServerTime();
            if (cashDisbursement.transtype == DisbursementTransType.cashin)
                cashDisbursement.refaccountid = null;
            _dataContext.Add(cashDisbursement);
            await _dataContext.SaveChangesAsync();
            if (cashDisbursement.transtype == DisbursementTransType.transferout)
            {
                await _accountService.UpdateOfficeBalanceAsync(cashDisbursement.accountid, cashDisbursement.amount * -1);
                var refDisbursement = await _dataContext.cashDisbursements.AsNoTracking().FirstOrDefaultAsync(x => x.disbursementid == cashDisbursement.disbursementid);
                refDisbursement.disbursementid = 0;
                refDisbursement.accountid = cashDisbursement.refaccountid ?? 0;
                refDisbursement.refaccountid = cashDisbursement.accountid;
                refDisbursement.transtype = DisbursementTransType.transferin;
                refDisbursement.refdisbursementid = cashDisbursement.disbursementid;
                _dataContext.Add(refDisbursement);
                await _dataContext.SaveChangesAsync();
                cashDisbursement.refdisbursementid = refDisbursement.disbursementid;
                await _dataContext.SaveChangesAsync();
                await _accountService.UpdateOfficeBalanceAsync(refDisbursement.accountid, refDisbursement.amount);
            }
            else
                await _accountService.UpdateOfficeBalanceAsync(cashDisbursement.accountid, cashDisbursement.amount);
            await _logService.SaveLogAsync(cashDisbursement.companyid, cashDisbursement.createdbyid, AppModule.PettyCash, "Petty Cash Disbursement", "Create New Record : " + cashDisbursement.disbursementid, 0);
            return _mapper.Map<CashDisbursementViewModel>(cashDisbursement);
        }

        public async Task<ICollection<CashDisbursementViewModel>> GetCashDisbursements(int companyID, string search, DateTime startDate, DateTime endDate)
        {
            var result = await _dataContext.cashDisbursements.Include(x => x.office).Include(x => x.refOffice)
                .Where(x => x.companyid == companyID && x.refdate >= startDate && x.refdate <= endDate
                    && (x.office.accountname.Contains(search) || x.refno.Contains(search)))
                .OrderByDescending(x => x.datecreated)
                .ToListAsync();
            return _mapper.Map<ICollection<CashDisbursementViewModel>>(result);
        }

        private async Task<CashDisbursement> GetDisbursement(long id)
        {
            return await _dataContext.cashDisbursements.Include(x => x.office).Include(x => x.refOffice).FirstOrDefaultAsync(x => x.disbursementid == id);
        }
        public async Task<CashDisbursementViewModel> GetDisbursementByID(long id)
        {
            return _mapper.Map<CashDisbursementViewModel>(await GetDisbursement(id));
        }

        public async Task<bool> VoidDisbursement(long id, bool isRefDisbursement, long employeeid)
        {
            var data = await GetDisbursement(id);
            if (data != null)
            {
                data.status = DisbursementStatus.@void;
                if (data.refdisbursementid > 0 && !isRefDisbursement)
                    await VoidDisbursement(data.refdisbursementid, true, employeeid);

                if (data.transtype == DisbursementTransType.transferout)
                    await _accountService.UpdateOfficeBalanceAsync(data.accountid, data.amount);
                else
                    await _accountService.UpdateOfficeBalanceAsync(data.accountid, data.amount * -1);
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(data.companyid, employeeid, AppModule.PettyCash, "Petty Cash Disbursement", "Void Record : " + id, 0);
                return true;
            }
            return false;
        }

        public async Task ReComputePettyCash(long accountID)
        {
            var cashIns = await _dataContext.cashDisbursements.Where(x => x.accountid == accountID
                && (x.transtype == DisbursementTransType.cashin || x.transtype == DisbursementTransType.transferin) && x.status == DisbursementStatus.approved)
                .SumAsync(x => x.amount);
            var cashOuts = await _dataContext.cashDisbursements.Where(x => x.accountid == accountID
                && x.transtype == DisbursementTransType.transferout && x.status == DisbursementStatus.approved)
                .SumAsync(x => x.amount);
            var expenses = await _dataContext.expenses.Where(x => x.accountid == accountID
                && x.status == ExpenseStatus.approved && x.usepettycash).SumAsync(x => x.amount);
            var entity = await _dataContext.offices.FindAsync(accountID);
            entity.pettycashbalance = cashIns - cashOuts - expenses;
            await _dataContext.SaveChangesAsync();
        }

        public async Task<decimal> TotalPettyCashBalance(int companyID)
        {
            return await _dataContext.offices.Where(x => x.companyid == companyID && x.isactive).Select(x => x.pettycashbalance).SumAsync();
        }

        public async Task<ICollection<PettyCashAccountHistoryViewModel>> GetAccountHistory(int companyID, long accountID, DateTime startDate, DateTime endDate)
        {
            var result = await _dataContext.pettyCashAccountHistories.FromSqlRaw($"exec spWebReports @operation=0, @soperation=4, @search = '{accountID}', @companyid = {companyID}, @startdate = '{startDate}', @enddate = '{endDate}'").ToListAsync();
            return _mapper.Map<ICollection<PettyCashAccountHistoryViewModel>>(result);
        }
    }
}
