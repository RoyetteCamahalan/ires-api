using AutoMapper;
using ires.Domain.Contracts;
using ires.Domain.DTO.AccountPayable;
using ires.Domain.DTO.Expense;
using ires.Domain.DTO.ExpenseType;
using ires.Domain.DTO.Vendor;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using ires.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class ExpenseRepository : IExpenseService
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public ExpenseRepository(DataContext dataContext, IAccountService accountService, IMapper mapper, ILogService logService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _mapper = mapper;
            _logService = logService;
        }
        public async Task<ExpenseViewModel> Create(ExpenseRequestDto requestDto)
        {
            var entity = _mapper.Map<Expense>(requestDto);
            entity.expenseid = 0;
            entity.transno = (await _dataContext.expenses.MaxAsync(x => (long?)x.transno) ?? 0) + 1;
            entity.transdate = DateTime.Now;
            entity.status = ExpenseStatus.approved;
            _dataContext.expenses.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(entity.companyid, entity.createdbyid, AppModule.Expenses, "Expense", "Create New Record : " + entity.expenseid, 0);
            if (entity.usepettycash)
                await _accountService.UpdateOfficeBalanceAsync(entity.accountid, entity.amount * -1);
            await this.ReComputeAPAsync(entity.payeeid);
            return _mapper.Map<ExpenseViewModel>(entity);
        }

        private async Task<Expense> GetExpenseByIDAsync(long ID)
        {
            return await _dataContext.expenses.Include(x => x.office).Include(x => x.expenseType).Include(x => x.vendor).FirstOrDefaultAsync(x => x.expenseid == ID);
        }

        public async Task<ExpenseViewModel> GetExpenseByID(long ID)
        {
            var result = await GetExpenseByIDAsync(ID);
            return _mapper.Map<ExpenseViewModel>(result);
        }

        public async Task<ICollection<ExpenseViewModel>> GetExpenses(int companyID, string search, DateTime startDate, DateTime endDate)
        {
            var result = await _dataContext.expenses.Include(x => x.office).Include(x => x.expenseType).Include(x => x.vendor)
                .Where(x => x.companyid == companyID && x.refdate >= startDate.Date && x.refdate <= endDate.Date &&
                    (x.office.accountname.Contains(search) || x.expenseType.expensetypedesc.Contains(search) || x.vendor.vendorname.Contains(search)))
                .OrderByDescending(x => x.transdate).ToListAsync();
            return _mapper.Map<ICollection<ExpenseViewModel>>(result);
        }

        public async Task<bool> Update(ExpenseRequestDto requestDto)
        {
            var data = await GetExpenseByIDAsync(requestDto.expenseid);
            if (data != null)
            {
                var oldAccountID = data.accountid;
                var oldVendorID = data.payeeid;
                var oldAmount = data.amount;
                var oldUsePettyCash = data.usepettycash;
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
                await _logService.SaveLogAsync(data.companyid, data.updatedbyid, AppModule.Expenses, "Expense", "Update Record ID : " + requestDto.expenseid, 0);
                if (oldAccountID != requestDto.accountid && oldUsePettyCash)
                    await _accountService.UpdateOfficeBalanceAsync(oldAccountID, oldAmount);

                if (oldVendorID != requestDto.payeeid)
                    await ReComputeAPAsync(oldVendorID);

                if (oldAccountID == requestDto.accountid && oldUsePettyCash && !requestDto.usepettycash)
                    await _accountService.UpdateOfficeBalanceAsync(requestDto.accountid, requestDto.amount);
                else if (oldAccountID == requestDto.accountid && !oldUsePettyCash && requestDto.usepettycash)
                    await _accountService.UpdateOfficeBalanceAsync(requestDto.accountid, requestDto.amount * -1);
                else if (oldAccountID == requestDto.accountid && oldUsePettyCash && requestDto.usepettycash)
                    await _accountService.UpdateOfficeBalanceAsync(requestDto.accountid, oldAmount - requestDto.amount);
                else if (oldAccountID != requestDto.accountid && requestDto.usepettycash)
                    await _accountService.UpdateOfficeBalanceAsync(requestDto.accountid, requestDto.amount * -1);
                return true;
            }
            return false;
        }
        public async Task<bool> VoidExpense(long id, long employeeid)
        {
            var data = await GetExpenseByIDAsync(id);
            if (data != null)
            {
                data.status = ExpenseStatus.@void;
                data.balance = 0;
                data.runningbalance = 0;
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(data.companyid, employeeid, AppModule.Expenses, "Expense", "Void Record : " + id, 0);
                if (data.usepettycash)
                    await _accountService.UpdateOfficeBalanceAsync(data.accountid, data.amount);

                await ReComputeAPAsync(data.payeeid);
                return true;
            }
            return false;
        }
        public async Task ReComputeAPAsync(long vendorID)
        {
            await Task.Run(() =>
                _dataContext.Database.ExecuteSqlRawAsync("exec spComputeExpense @vendorid = {0}", vendorID));
        }


        #region "ExpenseTypes"
        public async Task<ExpenseTypeViewModel> CreateExpenseType(ExpenseTypeRequestDto requestDto)
        {
            var entity = _mapper.Map<ExpenseType>(requestDto);
            entity.expensetypeid = 0;
            entity.datecreated = DateTime.Now;
            _dataContext.expenseTypes.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(entity.companyid, entity.createdbyid, AppModule.Expenses, "Expense Type", "Create New Expense Type : " + entity.expensetypeid + "-" + entity.expensetypedesc, 0);
            return _mapper.Map<ExpenseTypeViewModel>(entity);
        }

        private async Task<ExpenseType> GetExpenseTypeByIDAsync(long ID)
        {
            return await _dataContext.expenseTypes.Include(x => x.category).FirstOrDefaultAsync(x => x.expensetypeid == ID);
        }

        public async Task<ExpenseTypeViewModel> GetExpenseTypeByID(long ID)
        {
            var result = await GetExpenseTypeByIDAsync(ID);
            return _mapper.Map<ExpenseTypeViewModel>(result);
        }

        public async Task<ExpenseTypeViewModel> GetExpenseTypeByName(int companyID, string name)
        {
            var result = await _dataContext.expenseTypes.FirstOrDefaultAsync(x => x.companyid == companyID && x.expensetypedesc == name);
            return _mapper.Map<ExpenseTypeViewModel>(result);
        }

        public async Task<ICollection<ExpenseTypeViewModel>> GetExpenseTypes(int companyID, bool viewAll, string search)
        {
            var result = await _dataContext.expenseTypes.Include(x => x.category).Where(x => x.companyid == companyID && (x.isactive || viewAll) && x.expensetypedesc.Contains(search))
                .OrderBy(x => x.expensetypedesc).ToListAsync();
            return _mapper.Map<ICollection<ExpenseTypeViewModel>>(result);
        }

        public async Task<bool> UpdateExpenseType(ExpenseTypeRequestDto requestDto)
        {
            var entity = await GetExpenseTypeByIDAsync(requestDto.expensetypeid);
            if (entity != null)
            {
                entity.expensetypedesc = requestDto.expensetypedesc;
                entity.expensetypecat = requestDto.expensetypecat;
                entity.isactive = requestDto.isactive;
                entity.updatedbyid = requestDto.updatedbyid;
                entity.dateupdated = DateTime.Now;
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(entity.companyid, entity.updatedbyid, AppModule.Expenses, "Expense Type", "Update Expense Type ID : " + entity.expensetypeid + "-" + entity.expensetypedesc, 0);
                return true;
            }
            return false;
        }
        #endregion

        #region "Vendor"
        public async Task<VendorViewModel> CreateVendor(VendorRequestDto requestDto)
        {
            var entity = _mapper.Map<Vendor>(requestDto);
            entity.vendorid = 0;
            entity.datecreated = DateTime.Now;
            _dataContext.vendors.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(entity.companyid, entity.createdbyid, AppModule.Vendors, "Vendor", "Create New Vendor : " + entity.vendorid + "-" + entity.vendorname, 0);
            return _mapper.Map<VendorViewModel>(entity);
        }

        private async Task<Vendor> GetVendorByIDAsync(long ID)
        {
            return await _dataContext.vendors.FirstOrDefaultAsync(x => x.vendorid == ID);
        }

        public async Task<VendorViewModel> GetVendorByID(long ID)
        {
            return _mapper.Map<VendorViewModel>(await GetVendorByIDAsync(ID));
        }

        public async Task<VendorViewModel> GetVendorByName(int companyID, string name)
        {
            var result = await _dataContext.vendors.FirstOrDefaultAsync(x => x.companyid == companyID && x.vendorname == name);
            return _mapper.Map<VendorViewModel>(result);
        }

        public async Task<ICollection<VendorViewModel>> GetVendors(int companyID, bool viewAll, string search)
        {
            var result = await _dataContext.vendors.Where(x => x.companyid == companyID
                && (x.isactive || viewAll)
                && (x.vendorname.Contains(search) || x.tinno.Contains(search)))
                .OrderBy(x => x.vendorname).ToListAsync();
            if (result.Count == 0 && search == "" && viewAll)
            {
                var vendorSeeder = new VendorSeeder(_dataContext);
                await vendorSeeder.Seed(companyID);
                return await GetVendors(companyID, viewAll, search);
            }
            return _mapper.Map<ICollection<VendorViewModel>>(result);
        }

        public async Task<bool> UpdateVendor(VendorRequestDto requestDto)
        {
            var entity = await GetVendorByIDAsync(requestDto.vendorid);
            if (entity != null)
            {
                entity.vendorname = requestDto.vendorname;
                entity.address = requestDto.address;
                entity.contactno = requestDto.contactno;
                entity.tinno = requestDto.tinno;
                entity.isactive = requestDto.isactive;
                entity.updatedbyid = requestDto.updatedbyid;
                entity.dateupdated = DateTime.Now;
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(entity.companyid, entity.createdbyid, AppModule.Vendors, "Vendor", "Update Vendor ID : " + requestDto.vendorid + "-" + entity.vendorname, 0);
                return true;
            }
            return false;
        }

        public async Task<int> CountVendors(int companyID)
        {
            return await _dataContext.vendors.Where(x => x.companyid == companyID).CountAsync();
        }

        public async Task<AccountPayableViewModel> CreateAccountPayable(AccountPayableRequestDto requestDto)
        {
            var entity = _mapper.Map<AccountPayable>(requestDto);
            entity.chargeid = 0;
            entity.datecreated = DateTime.Now;
            entity.dateposted = entity.datecreated;
            _dataContext.accountPayables.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(entity.companyid, entity.createdbyid, AppModule.AccountsPayable, "Accounts Payable", "Create New Record : " + entity.chargeid, 0);
            return _mapper.Map<AccountPayableViewModel>(entity);
        }

        public async Task<bool> UpdateAccountPayable(AccountPayableRequestDto requestDto)
        {
            var entity = await GetAccountPayable(requestDto.chargeid);
            if (entity != null)
            {
                var oldVendorID = entity.vendorid;
                entity.vendorid = requestDto.vendorid;
                entity.actualdate = requestDto.actualdate;
                entity.expensetypeid = requestDto.expensetypeid;
                entity.refno = requestDto.refno;
                entity.memo = requestDto.memo;
                entity.amount = requestDto.amount;
                entity.updatedbyid = requestDto.updatedbyid;
                entity.dateupdated = DateTime.Now;
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(entity.companyid, entity.updatedbyid, AppModule.AccountsPayable, "Accounts Payable", "Update Record ID : " + requestDto.chargeid, 0);
                if (oldVendorID != entity.vendorid)
                    await ReComputeAPAsync(oldVendorID);
                await ReComputeAPAsync(entity.vendorid);
                return true;
            }
            return false;
        }

        private async Task<AccountPayable> GetAccountPayable(long id)
        {
            return await _dataContext.accountPayables.Include(x => x.expenseType).Include(x => x.vendor).FirstOrDefaultAsync(x => x.chargeid == id);
        }

        public async Task<AccountPayableViewModel> GetAccountPayableByID(long id)
        {
            return _mapper.Map<AccountPayableViewModel>(await GetAccountPayable(id));
        }

        public async Task<bool> VoidAccountPayable(long id, long employeeid)
        {
            var entity = await GetAccountPayableByID(id);
            if (entity != null)
            {
                entity.status = AccountPayableStatus.@void;
                entity.balance = 0;
                entity.runningbalance = 0;
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(entity.companyid, employeeid, AppModule.AccountsPayable, "Accounts Payable", "Void Record : " + id, 0);
                await ReComputeAPAsync(entity.vendorid);
                return true;
            }
            return false;
        }

        public async Task<ICollection<AccountPayableViewModel>> GetAccountPayables(int companyID, string search, DateTime startDate, DateTime endDate)
        {
            var result = await _dataContext.accountPayables.Include(x => x.expenseType).Include(x => x.vendor)
                .Where(x => x.companyid == companyID && x.actualdate >= startDate && x.actualdate <= endDate
                    && (x.vendor.vendorname.Contains(search) || x.refno.Contains(search) || x.memo.Contains(search)))
                .OrderByDescending(x => x.datecreated)
                .ToListAsync();
            return _mapper.Map<ICollection<AccountPayableViewModel>>(result);
        }
        #endregion
    }
}
