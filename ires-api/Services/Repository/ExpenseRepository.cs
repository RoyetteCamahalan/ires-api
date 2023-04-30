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

        public ExpenseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

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
