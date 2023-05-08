using ires_api.Data;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using ires_api.Services.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
{
    public class ExpenseRepository : IExpenseService
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;

        public ExpenseRepository(DataContext dataContext, IAccountService accountService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
        }
        public async Task<Expense> Create(Expense data)
        {
            data.expenseid = 0;
            data.transdate = DateTime.Now;
            data.status = Constants.ExpenseStatus.approved;
            _dataContext.expenses.Add(data);
            await _dataContext.SaveChangesAsync();
            await this.ReComputeAPAsync(data.payeeid);
            return data;
        }

        public async Task<Expense> GetExpenseByID(long ID)
        {
            return await _dataContext.expenses.Include(x => x.office).Include(x => x.expenseType).Include(x => x.vendor).FirstOrDefaultAsync(x => x.expensetypeid == ID);
        }

        public async Task<ICollection<Expense>> GetExpenses(int companyID, string search)
        {
            return await _dataContext.expenses.Include(x => x.office).Include(x => x.expenseType).Include(x => x.vendor)
                .Where(x => x.companyid == companyID &&
                    (x.office.accountname.Contains(search) || x.expenseType.expensetypedesc.Contains(search) || x.vendor.vendorname.Contains(search)))
                .OrderByDescending(x => x.transdate).ToListAsync();
        }

        public async Task<Expense> Update(ExpenseRequestDto requestDto)
        {
            var data = await GetExpenseByID(requestDto.expenseid);
            if (data != null)
            {
                var oldAccountID = data.accountid;
                var oldAmount = data.amount;
                data.accountid = requestDto.accountid;
                data.expensetypeid = requestDto.expensetypeid;
                data.refno = requestDto.refno;
                data.refdate = requestDto.refdate;
                data.amount = requestDto.amount;
                data.memo = requestDto.memo;
                data.payeeid = requestDto.payeeid;
                data.usepettycash = requestDto.usepettycash;
                data.updatedbyid = requestDto.updatedbyid;
                data.dateupdated = DateTime.Now;
                await _dataContext.SaveChangesAsync();
                if (oldAccountID != requestDto.accountid)
                {
                    _accountService.UpdateOfficeBalance(oldAccountID, oldAmount);
                    _accountService.UpdateOfficeBalance(requestDto.accountid, requestDto.amount * -1);
                }
                else
                    _accountService.UpdateOfficeBalance(requestDto.accountid, oldAmount - requestDto.amount);

            }
            return data;
        }
        public async Task ReComputeAPAsync(long vendorID)
        {
            await Task.Run(() =>
                _dataContext.Database.ExecuteSqlRawAsync("exec spComputeExpense @vendorid = {0}", vendorID));
        }


        #region "ExpenseTypes"
        public async Task<ExpenseType> CreateExpenseType(ExpenseType expenseType)
        {
            expenseType.expensetypeid = 0;
            expenseType.datecreated = DateTime.Now;
            _dataContext.expenseTypes.Add(expenseType);
            await _dataContext.SaveChangesAsync();
            return expenseType;
        }

        public async Task<ExpenseType> GetExpenseTypeByID(long ID)
        {
            return await _dataContext.expenseTypes.Include(x => x.category).FirstOrDefaultAsync(x => x.expensetypeid == ID);
        }

        public async Task<ExpenseType> GetExpenseTypeByName(int companyID, string name)
        {
            return await _dataContext.expenseTypes.FirstOrDefaultAsync(x => x.companyid == companyID && x.expensetypedesc == name);
        }

        public async Task<ICollection<ExpenseType>> GetExpenseTypes(int companyID, bool viewAll, string search)
        {
            return await _dataContext.expenseTypes.Include(x => x.category).Where(x => x.companyid == companyID && (x.isactive || viewAll) && x.expensetypedesc.Contains(search))
                .OrderBy(x => x.expensetypedesc).ToListAsync();
        }

        public async Task<ExpenseType> UpdateExpenseType(ExpenseTypeRequestDto requestDto)
        {
            var expenseType = await GetExpenseTypeByID(requestDto.expensetypeid);
            if (expenseType != null)
            {
                expenseType.expensetypedesc = requestDto.expensetypedesc;
                expenseType.expensetypecat = requestDto.expensetypecat;
                expenseType.isactive = requestDto.isactive;
                expenseType.updatedbyid = requestDto.updatedbyid;
                expenseType.dateupdated = DateTime.Now;
                await _dataContext.SaveChangesAsync();
            }
            return expenseType;
        }
        #endregion

        #region "Vendor"
        public async Task<Vendor> CreateVendor(Vendor vendor)
        {
            vendor.vendorid = 0;
            vendor.datecreated = DateTime.Now;
            _dataContext.vendors.Add(vendor);
            await _dataContext.SaveChangesAsync();
            return vendor;
        }

        public async Task<Vendor> GetVendorByID(long ID)
        {
            return await _dataContext.vendors.FirstOrDefaultAsync(x => x.vendorid == ID);
        }

        public async Task<Vendor> GetVendorByName(int companyID, string name)
        {
            return await _dataContext.vendors.FirstOrDefaultAsync(x => x.companyid == companyID && x.vendorname == name);
        }

        public async Task<ICollection<Vendor>> GetVendors(int companyID, bool viewAll, string search)
        {
            var result = await _dataContext.vendors.Where(x => x.companyid == companyID
                && ((x.isactive || viewAll))
                && (x.vendorname.Contains(search) || x.tinno.Contains(search)))
                .OrderBy(x => x.vendorname).ToListAsync();
            if (result.Count == 0 && search == "" && viewAll)
            {
                var vendorSeeder = new VendorSeeder(_dataContext);
                await vendorSeeder.Seed(companyID);
                return await GetVendors(companyID, viewAll, search);
            }
            return result;
        }

        public async Task<Vendor> UpdateVendor(VendorRequestDto requestDto)
        {
            var vendor = await GetVendorByID(requestDto.vendorid);
            if (vendor != null)
            {
                vendor.vendorname = requestDto.vendorname;
                vendor.address = requestDto.address;
                vendor.contactno = requestDto.contactno;
                vendor.tinno = requestDto.tinno;
                vendor.isactive = requestDto.isactive;
                vendor.updatedbyid = requestDto.updatedbyid;
                vendor.dateupdated = DateTime.Now;
                await _dataContext.SaveChangesAsync();
            }
            return vendor;
        }

        public async Task<AccountPayable> CreateAccountPayable(AccountPayable data)
        {
            data.chargeid = 0;
            data.datecreated = DateTime.Now;
            data.dateposted = data.datecreated;
            _dataContext.accountPayables.Add(data);
            await _dataContext.SaveChangesAsync();
            return data;
        }

        public async Task<AccountPayable> UpdateAccountPayable(AccountPayableRequestDto requestDto)
        {
            var data = await GetAccountPayableByID(requestDto.chargeid);
            if (data != null)
            {
                var oldVendorID = data.vendorid;
                data.vendorid = requestDto.vendorid;
                data.actualdate = requestDto.actualdate;
                data.expensetypeid = requestDto.expensetypeid;
                data.refno = requestDto.refno;
                data.memo = requestDto.memo;
                data.amount = requestDto.amount;
                data.updatedbyid = requestDto.updatedbyid;
                data.dateupdated = DateTime.Now;
                await _dataContext.SaveChangesAsync();
                if (oldVendorID != data.vendorid)
                    await ReComputeAPAsync(oldVendorID);
                await ReComputeAPAsync(data.vendorid);
            }
            return data;
        }

        public async Task<AccountPayable> GetAccountPayableByID(long id)
        {
            return await _dataContext.accountPayables.Include(x => x.expenseType).Include(x => x.vendor).FirstOrDefaultAsync(x => x.chargeid == id);
        }

        public async Task<bool> VoidAccountPayable(long id)
        {
            var data = await GetAccountPayableByID(id);
            if (data != null)
            {
                data.status = Constants.AccountPayableStatus.@void;
                data.balance = 0;
                data.runningbalance = 0;
                await _dataContext.SaveChangesAsync();
                await ReComputeAPAsync(data.vendorid);
                return true;
            }
            return false;
        }

        public async Task<ICollection<AccountPayable>> GetAccountPayables(int companyID, string search, DateTime startDate, DateTime endDate)
        {
            return await _dataContext.accountPayables.Include(x => x.expenseType).Include(x => x.vendor)
                .Where(x => x.companyid == companyID && x.actualdate >= startDate && x.actualdate <= endDate
                    && (x.vendor.vendorname.Contains(search) || x.refno.Contains(search) || x.memo.Contains(search)))
                .OrderByDescending(x => x.datecreated)
                .ToListAsync();
        }
        #endregion
    }
}
