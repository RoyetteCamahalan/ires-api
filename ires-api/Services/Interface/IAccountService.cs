using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IAccountService
    {
        public Task<BankAccount> GetBankAccountByID(long id);
        public Task<BankAccount> CreateBankAccountAsync(BankAccount requestDto);
        public Task<BankAccount> UpdateBankAccountAsync(BankAccountRequestDto requestDto);
        public Task<ICollection<BankAccount>> GetBankAccounts(int companyID, string search);
        public Task<bool> isBankAccountExist(BankAccountRequestDto requestDto);

        public Task<Office> CreateOfficeAsync(Office Office);
        public Task<Office> UpdateOfficeAsync(OfficeRequestDto requestDto);
        public Task UpdateOfficeBalanceAsync(long id, decimal addedAmount);
        public Task<Office> GetOfficeByID(long ID);
        public Task<Office> GetOfficeByName(int companyID, string name);
        public Task<ICollection<Office>> GetOffices(int companyID, string search, bool viewAll);

    }
}
