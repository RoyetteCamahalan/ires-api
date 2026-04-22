using ires.Domain.DTO.AccountPayable;
using ires.Domain.DTO.Expense;
using ires.Domain.DTO.ExpenseType;
using ires.Domain.DTO.Vendor;

namespace ires.Domain.Contracts
{
    public interface IExpenseService
    {
        public Task<ExpenseViewModel> Create(ExpenseRequestDto requestDto);
        public Task<bool> Update(ExpenseRequestDto requestDto);
        public Task<ExpenseViewModel> GetExpenseByID(long ID);
        public Task<ICollection<ExpenseViewModel>> GetExpenses(int companyID, string search, DateTime startDate, DateTime endDate);
        public Task<bool> VoidExpense(long id, long employeeid);
        public Task ReComputeAPAsync(long vendorID);

        public Task<ExpenseTypeViewModel> CreateExpenseType(ExpenseTypeRequestDto requestDto);
        public Task<bool> UpdateExpenseType(ExpenseTypeRequestDto requestDto);
        public Task<ExpenseTypeViewModel> GetExpenseTypeByID(long ID);
        public Task<ExpenseTypeViewModel> GetExpenseTypeByName(int companyID, string name);
        public Task<ICollection<ExpenseTypeViewModel>> GetExpenseTypes(bool viewAll, string search);


        public Task<VendorViewModel> CreateVendor(VendorRequestDto requestDto);
        public Task<bool> UpdateVendor(VendorRequestDto requestDto);
        public Task<VendorViewModel> GetVendorByID(long ID);
        public Task<VendorViewModel> GetVendorByName(int companyID, string name);
        public Task<int> CountVendors(int companyID);
        public Task<ICollection<VendorViewModel>> GetVendors(int companyID, bool viewAll, string search);


        public Task<AccountPayableViewModel> CreateAccountPayable(AccountPayableRequestDto requestDto);
        public Task<bool> UpdateAccountPayable(AccountPayableRequestDto requestDto);
        public Task<AccountPayableViewModel> GetAccountPayableByID(long id);
        public Task<bool> VoidAccountPayable(long id, long employeeid);
        public Task<ICollection<AccountPayableViewModel>> GetAccountPayables(int companyID, string search, DateTime startDate, DateTime endDate);
    }
}
