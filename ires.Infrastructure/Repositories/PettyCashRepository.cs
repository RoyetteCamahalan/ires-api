using AutoMapper;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.CashDisbursement;
using ires.Domain.DTO.PettyCash;
using ires.Domain.Enumerations;
using ires.Infrastructure.Extensions;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace ires.Infrastructure.Repositories
{
    public class PettyCashRepository(
        DataContext _dataContext,
        IAccountService _accountService,
        IMapper _mapper,
        ILogService _logService,
        ICurrentUserService _currentUserService) : IPettyCashService
    {

        public async Task<CashDisbursementViewModel> Create(CashDisbursementRequestDto requestDto)
        {
            var cashDisbursement = _mapper.Map<CashDisbursement>(requestDto);
            cashDisbursement.disbursementid = 0;
            cashDisbursement.companyid = _currentUserService.companyid;
            cashDisbursement.createdbyid = _currentUserService.employeeid;
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
            await _logService.SaveLogAsync(AppModule.PettyCash, "Petty Cash Disbursement", "Create New Record : " + cashDisbursement.disbursementid, 0);
            return _mapper.Map<CashDisbursementViewModel>(cashDisbursement);
        }

        public async Task<PaginatedResult<CashDisbursementViewModel>> GetCashDisbursements(PaginationRequest request)
        {
            var query = _dataContext.cashDisbursements.Include(x => x.office).Include(x => x.refOffice)
                .Where(x => x.companyid == _currentUserService.companyid && x.refdate >= request.startDate && x.refdate <= request.endDate
                    && (x.office.accountname.Contains(request.searchString) || x.refno.Contains(request.searchString)))
                .OrderByDescending(x => x.datecreated).AsQueryable();
            return await query.AsPaginatedResult<CashDisbursement, CashDisbursementViewModel>(request, _mapper.ConfigurationProvider);
        }

        private async Task<CashDisbursement> GetDisbursement(long id)
        {
            return await _dataContext.cashDisbursements.Include(x => x.office).Include(x => x.refOffice)
                .FirstOrDefaultAsync(x => x.disbursementid == id);
        }
        public async Task<CashDisbursementViewModel> GetDisbursementByID(long id)
        {
            return _mapper.Map<CashDisbursementViewModel>(await GetDisbursement(id));
        }

        public async Task VoidDisbursement(long id, bool isRefDisbursement)
        {
            var data = await GetDisbursementByID(id);
            if (data != null)
            {
                data.status = DisbursementStatus.@void;
                if (data.refdisbursementid > 0 && !isRefDisbursement)
                    await VoidDisbursement(data.refdisbursementid, true);

                if (data.transtype == DisbursementTransType.transferout)
                    await _accountService.UpdateOfficeBalanceAsync(data.accountid, data.amount);
                else
                    await _accountService.UpdateOfficeBalanceAsync(data.accountid, data.amount * -1);
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(AppModule.PettyCash, "Petty Cash Disbursement", "Void Record : " + id, 0);
            }
        }

        public async Task ReComputePettyCash(long accountID)
        {
            var parameter = new List<SqlParameter>
            {
                new SqlParameter("@operation", 2),
                new SqlParameter("@soperation", 0),
                new SqlParameter("@search", accountID.ToString())
            };
            await Task.Run(() =>
                _dataContext.Database.ExecuteSqlRawAsync("exec spPettyCash @operation, @soperation, @search", parameter));
        }

        public async Task<decimal> TotalPettyCashBalance()
        {
            return await _dataContext.offices.Where(x => x.companyid == _currentUserService.companyid && x.isactive)
                .Select(x => x.pettycashbalance).SumAsync();
        }

        public async Task<ICollection<PettyCashAccountHistoryViewModel>> GetAccountHistory(long accountID, DateTime startDate, DateTime endDate)
        {
            var result = await _dataContext.pettyCashAccountHistories
                .FromSqlRaw("exec spWebReports @operation = 0, @soperation = 4, @search = {0}, @companyid = {1}, @startdate = {2}, @enddate = {3}",
                accountID, _currentUserService.companyid, startDate, endDate).ToListAsync();
            return _mapper.Map<ICollection<PettyCashAccountHistoryViewModel>>(result);
        }
    }
}
