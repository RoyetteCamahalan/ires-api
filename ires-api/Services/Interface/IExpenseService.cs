using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IExpenseService
    {
        public ExpenseType CreateExpenseType(ExpenseType expenseType);
        public ExpenseType UpdateExpenseType(ExpenseTypeRequestDto requestDto);
        public ExpenseType GetExpenseTypeByID(long ID);
        public ExpenseType GetExpenseTypeByName(int companyID, string name);
        public ICollection<ExpenseType> GetExpenseTypes(int companyID, string search);


        public Vendor CreateVendor(Vendor vendor);
        public Vendor UpdateVendor(VendorRequestDto requestDto);
        public Vendor GetVendorByID(long ID);
        public Vendor GetVendorByName(int companyID, string name);
        public ICollection<Vendor> GetVendors(int companyID, string search);
    }
}
