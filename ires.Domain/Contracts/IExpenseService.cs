using ires.Domain.Common;
using ires.Domain.DTO.AccountPayable;
using ires.Domain.DTO.Expense;
using ires.Domain.DTO.ExpenseType;
using ires.Domain.DTO.Vendor;

namespace ires.Domain.Contracts
{
    public interface IExpenseService
    {
        public Task<ExpenseViewModel> Create(ExpenseRequestDto requestDto);
        public Task Update(ExpenseRequestDto requestDto);
        public Task<ExpenseViewModel> GetExpenseByID(long ID);
        public Task<PaginatedResult<ExpenseViewModel>> GetExpenses(PaginationRequest request);
        public Task VoidExpense(long id);
        public Task ReComputeAPAsync(long vendorID);

        public Task<ExpenseTypeViewModel> CreateExpenseType(ExpenseTypeRequestDto requestDto);
        public Task UpdateExpenseType(ExpenseTypeRequestDto requestDto);
        public Task<ExpenseTypeViewModel> GetExpenseTypeByID(long ID);
        public Task<ExpenseTypeViewModel> GetExpenseTypeByName(string name);
        public Task<PaginatedResult<ExpenseTypeViewModel>> GetExpenseTypes(PaginationRequest request);


        public Task<VendorViewModel> CreateVendor(VendorRequestDto requestDto);
        public Task UpdateVendor(VendorRequestDto requestDto);
        public Task<VendorViewModel> GetVendorByID(long ID);
        public Task<VendorViewModel> GetVendorByName(string name);
        public Task<int> CountVendors();
        public Task<PaginatedResult<VendorViewModel>> GetVendors(PaginationRequest request);


        public Task<AccountPayableViewModel> CreateAccountPayable(AccountPayableRequestDto requestDto);
        public Task UpdateAccountPayable(AccountPayableRequestDto requestDto);
        public Task<AccountPayableViewModel> GetAccountPayableByID(long id);
        public Task VoidAccountPayable(long id);
        public Task<PaginatedResult<AccountPayableViewModel>> GetAccountPayables(PaginationRequest request);
    }
}
