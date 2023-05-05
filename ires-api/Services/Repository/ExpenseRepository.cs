using ires_api.Data;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using ires_api.Services.Seeders;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

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
            var parameter = new List<SqlParameter>
            {
                new SqlParameter("@vendorid", vendorID)
            };
            await Task.Run(() =>
                _dataContext.Database.ExecuteSqlRawAsync("exec spComputeExpense @vendorid", parameter));
        }


        #region "ExpenseTypes"
        public ExpenseType CreateExpenseType(ExpenseType expenseType)
        {
            expenseType.expensetypeid = 0;
            expenseType.datecreated = DateTime.Now;
            _dataContext.expenseTypes.Add(expenseType);
            _dataContext.SaveChanges();
            return expenseType;
        }

        public ExpenseType GetExpenseTypeByID(long ID)
        {
            return _dataContext.expenseTypes.Include(x => x.category).FirstOrDefault(x => x.expensetypeid == ID);
        }

        public ExpenseType GetExpenseTypeByName(int companyID, string name)
        {
            return _dataContext.expenseTypes.FirstOrDefault(x => x.companyid == companyID && x.expensetypedesc == name);
        }

        public ICollection<ExpenseType> GetExpenseTypes(int companyID, string search)
        {
            return _dataContext.expenseTypes.Include(x => x.category).Where(x => x.companyid == companyID && x.expensetypedesc.Contains(search))
                .OrderBy(x => x.expensetypedesc).ToList();
        }

        public ExpenseType UpdateExpenseType(ExpenseTypeRequestDto requestDto)
        {
            var expenseType = GetExpenseTypeByID(requestDto.expensetypeid);
            if (expenseType != null)
            {
                expenseType.expensetypedesc = requestDto.expensetypedesc;
                expenseType.expensetypecat = requestDto.expensetypecat;
                expenseType.isactive = requestDto.isactive;
                expenseType.updatedbyid = requestDto.updatedbyid;
                expenseType.dateupdated = DateTime.Now;
                _dataContext.SaveChanges();
            }
            return expenseType;
        }
        #endregion

        #region "Vendor"
        public Vendor CreateVendor(Vendor vendor)
        {
            vendor.vendorid = 0;
            vendor.datecreated = DateTime.Now;
            _dataContext.vendors.Add(vendor);
            _dataContext.SaveChanges();
            return vendor;
        }

        public Vendor GetVendorByID(long ID)
        {
            return _dataContext.vendors.FirstOrDefault(x => x.vendorid == ID);
        }

        public Vendor GetVendorByName(int companyID, string name)
        {
            return _dataContext.vendors.FirstOrDefault(x => x.companyid == companyID && x.vendorname == name);
        }

        public ICollection<Vendor> GetVendors(int companyID, string search)
        {
            var result = _dataContext.vendors.Where(x => x.companyid == companyID && x.vendorname.Contains(search) && x.tinno.Contains(search))
                .OrderBy(x => x.vendorname).ToList();
            if (result.Count == 0 && search == "")
            {
                var vendorSeeder = new VendorSeeder(_dataContext);
                vendorSeeder.Seed(companyID);
                return GetVendors(companyID, search);
            }
            return result;
        }

        public Vendor UpdateVendor(VendorRequestDto requestDto)
        {
            var vendor = GetVendorByID(requestDto.vendorid);
            if (vendor != null)
            {
                vendor.vendorname = requestDto.vendorname;
                vendor.address = requestDto.address;
                vendor.contactno = requestDto.contactno;
                vendor.tinno = requestDto.tinno;
                vendor.isactive = requestDto.isactive;
                vendor.updatedbyid = requestDto.updatedbyid;
                vendor.dateupdated = DateTime.Now;
                _dataContext.SaveChanges();
            }
            return vendor;
        }
        #endregion
    }
}
