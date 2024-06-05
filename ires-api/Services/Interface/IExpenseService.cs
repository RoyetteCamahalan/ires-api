using ires_api.DTO.AccountPayable;
using ires_api.DTO.Expense;
using ires_api.DTO.ExpenseType;
using ires_api.DTO.Vendor;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IExpenseService
    {
        public Task<Expense> Create(Expense expense);
        public Task<Expense> Update(ExpenseRequestDto requestDto);
        public Task<Expense> GetExpenseByID(long ID);
        public Task<ICollection<Expense>> GetExpenses(int companyID, string search, DateTime startDate, DateTime endDate);
        public Task<bool> VoidExpense(long id);
        public Task ReComputeAPAsync(long vendorID);

        public Task<ExpenseType> CreateExpenseType(ExpenseType expenseType);
        public Task<ExpenseType> UpdateExpenseType(ExpenseTypeRequestDto requestDto);
        public Task<ExpenseType> GetExpenseTypeByID(long ID);
        public Task<ExpenseType> GetExpenseTypeByName(int companyID, string name);
        public Task<ICollection<ExpenseType>> GetExpenseTypes(int companyID, bool viewAll, string search);


        public Task<Vendor> CreateVendor(Vendor vendor);
        public Task<Vendor> UpdateVendor(VendorRequestDto requestDto);
        public Task<Vendor> GetVendorByID(long ID);
        public Task<Vendor> GetVendorByName(int companyID, string name);
        public Task<int> CountVendors(int companyID);
        public Task<ICollection<Vendor>> GetVendors(int companyID, bool viewAll, string search);


        public Task<AccountPayable> CreateAccountPayable(AccountPayable data);
        public Task<AccountPayable> UpdateAccountPayable(AccountPayableRequestDto requestDto);
        public Task<AccountPayable> GetAccountPayableByID(long id);
        public Task<bool> VoidAccountPayable(long id);
        public Task<ICollection<AccountPayable>> GetAccountPayables(int companyID, string search, DateTime startDate, DateTime endDate);
    }
}
