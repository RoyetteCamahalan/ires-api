using ires_api.Data;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace ires_api.Services.Repository
{
    public class PettyCashRepository : IPettyCashService
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;

        public PettyCashRepository(DataContext dataContext, IAccountService accountService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
        }
        public async Task<CashDisbursement> Create(CashDisbursement cashDisbursement)
        {
            cashDisbursement.disbursementid = 0;
            cashDisbursement.datecreated = DateTime.Now;
            _dataContext.Add(cashDisbursement);
            await _dataContext.SaveChangesAsync();
            if (cashDisbursement.transtype == Constants.DisbursementTransType.transferout)
            {
                await _accountService.UpdateOfficeBalanceAsync(cashDisbursement.accountid, cashDisbursement.amount * -1);
                var refDisbursement = await _dataContext.cashDisbursements.AsNoTracking().FirstOrDefaultAsync(x => x.disbursementid == cashDisbursement.disbursementid);
                refDisbursement.disbursementid = 0;
                refDisbursement.accountid = cashDisbursement.refaccountid;
                refDisbursement.refaccountid = cashDisbursement.accountid;
                refDisbursement.transtype = Constants.DisbursementTransType.transferin;
                refDisbursement.refdisbursementid = cashDisbursement.disbursementid;
                _dataContext.Add(refDisbursement);
                await _dataContext.SaveChangesAsync();
                cashDisbursement.refdisbursementid = refDisbursement.disbursementid;
                await _dataContext.SaveChangesAsync();
                await _accountService.UpdateOfficeBalanceAsync(refDisbursement.accountid, refDisbursement.amount);
            }
            else
                await _accountService.UpdateOfficeBalanceAsync(cashDisbursement.accountid, cashDisbursement.amount);
            return cashDisbursement;
        }

        public async Task<ICollection<CashDisbursement>> GetCashDisbursements(int companyID, string search, DateTime startDate, DateTime endDate)
        {
            return await _dataContext.cashDisbursements.Include(x => x.office).Include(x => x.refOffice)
                .Where(x => x.companyid == companyID && x.refdate >= startDate && x.refdate <= endDate
                    && (x.office.accountname.Contains(search) || x.refno.Contains(search)))
                .OrderByDescending(x => x.datecreated)
                .ToListAsync();
        }

        public async Task<CashDisbursement> GetDisbursementByID(long id)
        {
            return await _dataContext.cashDisbursements.Include(x => x.office).Include(x => x.refOffice).FirstOrDefaultAsync(x => x.disbursementid == id);
        }

        public async Task<bool> VoidDisbursement(long id, bool isRefDisbursement)
        {
            var data = await GetDisbursementByID(id);
            if (data != null)
            {
                data.status = Constants.DisbursementStatus.@void;
                if (data.refdisbursementid > 0 && !isRefDisbursement)
                    await VoidDisbursement(data.refdisbursementid, true);

                if (data.transtype == Constants.DisbursementTransType.transferout)
                    await _accountService.UpdateOfficeBalanceAsync(data.accountid, data.amount);
                else
                    await _accountService.UpdateOfficeBalanceAsync(data.accountid, data.amount * -1);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
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

    }
}
