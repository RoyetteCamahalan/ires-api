using AutoMapper;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.AccountPayable;
using ires.Domain.DTO.Expense;
using ires.Domain.DTO.ExpenseType;
using ires.Domain.DTO.Vendor;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using ires.Infrastructure.Extensions;
using ires.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class ExpenseRepository(
        DataContext _dataContext,
        IAccountService _accountService,
        IMapper _mapper,
        ILogService _logService,
        ICurrentUserService _currentUserService) : IExpenseService
    {

        public async Task<ExpenseViewModel> Create(ExpenseRequestDto requestDto)
        {
            var entity = _mapper.Map<Expense>(requestDto);
            entity.expenseid = 0;
            entity.transno = (await _dataContext.expenses.MaxAsync(x => (long?)x.transno) ?? 0) + 1;
            entity.transdate = Utility.GetServerTime();
            entity.status = ExpenseStatus.approved;
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            _dataContext.expenses.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Expenses, "Expense", "Create New Record : " + entity.expenseid, 0);
            if (entity.usepettycash)
                await _accountService.UpdateOfficeBalanceAsync(entity.accountid, entity.amount * -1);
            await this.ReComputeAPAsync(entity.payeeid);
            return _mapper.Map<ExpenseViewModel>(entity);
        }

        private async Task<Expense> GetExpenseByIDAsync(long ID)
        {
            return await _dataContext.expenses.Include(x => x.office)
                .Include(x => x.expenseType).Include(x => x.vendor)
                .FirstOrDefaultAsync(x => x.expenseid == ID) ?? throw new EntityNotFoundException();
        }

        public async Task<ExpenseViewModel> GetExpenseByID(long ID)
        {
            var result = await GetExpenseByIDAsync(ID);
            return _mapper.Map<ExpenseViewModel>(result);
        }

        public async Task<PaginatedResult<ExpenseViewModel>> GetExpenses(PaginationRequest request)
        {
            var query = _dataContext.expenses.Include(x => x.office)
                .Include(x => x.expenseType).Include(x => x.vendor)
                .Where(x => x.companyid == _currentUserService.companyid &&
                    x.refdate >= (request.startDate).Date
                    && x.refdate <= (request.endDate).Date &&
                    (x.office.accountname.Contains(request.searchString) || x.expenseType.expensetypedesc.Contains(request.searchString) || x.vendor.vendorname.Contains(request.searchString)))
                .OrderByDescending(x => x.transdate).AsQueryable();
            return await query.AsPaginatedResult<Expense, ExpenseViewModel>(request, _mapper.ConfigurationProvider);
        }

        public async Task Update(ExpenseRequestDto requestDto)
        {
            var data = await GetExpenseByIDAsync(requestDto.expenseid);
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
            data.updatedbyid = _currentUserService.employeeid;
            data.dateupdated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Expenses, "Expense", "Update Record ID : " + requestDto.expenseid);
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
        }
        public async Task VoidExpense(long id)
        {
            var data = await GetExpenseByIDAsync(id);
            data.status = ExpenseStatus.@void;
            data.balance = 0;
            data.runningbalance = 0;
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Expenses, "Expense", "Void Record : " + id);
            if (data.usepettycash)
                await _accountService.UpdateOfficeBalanceAsync(data.accountid, data.amount);

            await ReComputeAPAsync(data.payeeid);
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
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.expenseTypes.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Expenses, "Expense Type", "Create New Expense Type : " + entity.expensetypeid + "-" + entity.expensetypedesc);
            return _mapper.Map<ExpenseTypeViewModel>(entity);
        }

        private async Task<ExpenseType> GetExpenseTypeByIDAsync(long ID)
        {
            return await _dataContext.expenseTypes.Include(x => x.category)
                .FirstOrDefaultAsync(x => x.expensetypeid == ID) ?? throw new EntityNotFoundException();
        }

        public async Task<ExpenseTypeViewModel> GetExpenseTypeByID(long ID)
        {
            var result = await GetExpenseTypeByIDAsync(ID);
            return _mapper.Map<ExpenseTypeViewModel>(result);
        }

        public async Task<ExpenseTypeViewModel> GetExpenseTypeByName(string name)
        {
            var result = await _dataContext.expenseTypes.FirstOrDefaultAsync(x => x.companyid == _currentUserService.companyid && x.expensetypedesc == name);
            return _mapper.Map<ExpenseTypeViewModel>(result);
        }

        public async Task<PaginatedResult<ExpenseTypeViewModel>> GetExpenseTypes(PaginationRequest request)
        {
            var query = _dataContext.expenseTypes.Include(x => x.category)
                .Where(x => x.companyid == _currentUserService.companyid && (x.isactive || request.viewAll) && x.expensetypedesc.Contains(request.searchString))
                .OrderBy(x => x.expensetypedesc).AsQueryable();
            if (!query.Any() && request.search == "")
            {
                var expenseTypeSeeder = new ExpenseTypeSeeder(_dataContext);
                await expenseTypeSeeder.Seed(_currentUserService.companyid);
                return await GetExpenseTypes(request);
            }
            return await query.AsPaginatedResult<ExpenseType, ExpenseTypeViewModel>(request, _mapper.ConfigurationProvider); ;
        }

        public async Task UpdateExpenseType(ExpenseTypeRequestDto requestDto)
        {
            var entity = await GetExpenseTypeByIDAsync(requestDto.expensetypeid);
            entity.expensetypedesc = requestDto.expensetypedesc;
            entity.expensetypecat = requestDto.expensetypecat;
            entity.isactive = requestDto.isactive;
            entity.updatedbyid = _currentUserService.employeeid;
            entity.dateupdated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Expenses, "Expense Type", "Update Expense Type ID : " + entity.expensetypeid + "-" + entity.expensetypedesc);
        }
        #endregion

        #region "Vendor"
        public async Task<VendorViewModel> CreateVendor(VendorRequestDto requestDto)
        {
            var entity = _mapper.Map<Vendor>(requestDto);
            entity.vendorid = 0;
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.vendors.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Vendors, "Vendor", "Create New Vendor : " + entity.vendorid + "-" + entity.vendorname);
            return _mapper.Map<VendorViewModel>(entity);
        }

        private async Task<Vendor> GetVendorByIDAsync(long ID)
        {
            return await _dataContext.vendors.FirstOrDefaultAsync(x => x.vendorid == ID) ?? throw new EntityNotFoundException(); ;
        }

        public async Task<VendorViewModel> GetVendorByID(long ID)
        {
            return _mapper.Map<VendorViewModel>(await GetVendorByIDAsync(ID));
        }

        public async Task<VendorViewModel> GetVendorByName(string name)
        {
            var result = await _dataContext.vendors.FirstOrDefaultAsync(x => x.companyid == _currentUserService.companyid && x.vendorname == name);
            return _mapper.Map<VendorViewModel>(result);
        }

        public async Task<PaginatedResult<VendorViewModel>> GetVendors(PaginationRequest request)
        {
            var query = _dataContext.vendors.Where(x => x.companyid == _currentUserService.companyid
                && (x.isactive || request.viewAll)
                && (x.vendorname.Contains(request.searchString) || x.tinno.Contains(request.searchString)))
                .OrderBy(x => x.vendorname).AsQueryable();
            if (!query.Any() && request.search == "")
            {
                var vendorSeeder = new VendorSeeder(_dataContext);
                await vendorSeeder.Seed(_currentUserService.companyid);
                return await GetVendors(request);
            }
            return await query.AsPaginatedResult<Vendor, VendorViewModel>(request, _mapper.ConfigurationProvider);
        }

        public async Task UpdateVendor(VendorRequestDto requestDto)
        {
            var entity = await GetVendorByIDAsync(requestDto.vendorid);
            entity.vendorname = requestDto.vendorname;
            entity.address = requestDto.address;
            entity.contactno = requestDto.contactno;
            entity.tinno = requestDto.tinno;
            entity.isactive = requestDto.isactive;
            entity.updatedbyid = _currentUserService.employeeid;
            entity.dateupdated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Vendors, "Vendor", "Update Vendor ID : " + requestDto.vendorid + "-" + entity.vendorname);
        }

        public async Task<int> CountVendors()
        {
            return await _dataContext.vendors.Where(x => x.companyid == _currentUserService.companyid).CountAsync();
        }

        public async Task<AccountPayableViewModel> CreateAccountPayable(AccountPayableRequestDto requestDto)
        {
            var entity = _mapper.Map<AccountPayable>(requestDto);
            entity.chargeid = 0;
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            entity.dateposted = entity.datecreated;
            _dataContext.accountPayables.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.AccountsPayable, "Accounts Payable", "Create New Record : " + entity.chargeid);
            return _mapper.Map<AccountPayableViewModel>(entity);
        }

        public async Task UpdateAccountPayable(AccountPayableRequestDto requestDto)
        {
            var entity = await GetAccountPayable(requestDto.chargeid);
            var oldVendorID = entity.vendorid;
            entity.vendorid = requestDto.vendorid;
            entity.actualdate = requestDto.actualdate;
            entity.expensetypeid = requestDto.expensetypeid;
            entity.refno = requestDto.refno;
            entity.memo = requestDto.memo;
            entity.amount = requestDto.amount;
            entity.updatedbyid = _currentUserService.employeeid;
            entity.dateupdated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(entity.companyid, entity.updatedbyid, AppModule.AccountsPayable, "Accounts Payable", "Update Record ID : " + requestDto.chargeid, 0);
            if (oldVendorID != entity.vendorid)
                await ReComputeAPAsync(oldVendorID);
            await ReComputeAPAsync(entity.vendorid);
        }

        private async Task<AccountPayable> GetAccountPayable(long id)
        {
            return await _dataContext.accountPayables.Include(x => x.expenseType)
                .Include(x => x.vendor).FirstOrDefaultAsync(x => x.chargeid == id) ?? throw new EntityNotFoundException();
        }

        public async Task<AccountPayableViewModel> GetAccountPayableByID(long id)
        {
            return _mapper.Map<AccountPayableViewModel>(await GetAccountPayable(id));
        }

        public async Task VoidAccountPayable(long id)
        {
            var entity = await GetAccountPayableByID(id);
            entity.status = AccountPayableStatus.@void;
            entity.balance = 0;
            entity.runningbalance = 0;
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.AccountsPayable, "Accounts Payable", "Void Record : " + id);
            await ReComputeAPAsync(entity.vendorid);
        }

        public async Task<PaginatedResult<AccountPayableViewModel>> GetAccountPayables(PaginationRequest request)
        {
            var query = _dataContext.accountPayables.Include(x => x.expenseType).Include(x => x.vendor)
                .Where(x => x.companyid == _currentUserService.companyid && x.actualdate >= request.startDate && x.actualdate <= request.endDate
                    && (x.vendor.vendorname.Contains(request.searchString) || x.refno.Contains(request.searchString) || x.memo.Contains(request.searchString)))
                .OrderByDescending(x => x.datecreated).AsQueryable();
            return await query.AsPaginatedResult<AccountPayable, AccountPayableViewModel>(request, _mapper.ConfigurationProvider);
        }
        #endregion
    }
}
