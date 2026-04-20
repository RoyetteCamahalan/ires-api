using ires.Domain.DTO.BankAccount;
using ires.Domain.DTO.Office;

namespace ires.Domain.Contracts
{
    public interface IAccountService
    {
        public Task<BankAccountViewModel> GetBankAccountByID(long id);
        public Task<BankAccountViewModel> CreateBankAccountAsync(BankAccountRequestDto requestDto);
        public Task<bool> UpdateBankAccountAsync(BankAccountRequestDto requestDto);
        public Task<ICollection<BankAccountViewModel>> GetBankAccounts(string search);
        public Task<bool> isBankAccountExist(BankAccountRequestDto requestDto);

        public Task<OfficeViewModel> CreateOfficeAsync(OfficeRequestDto requestDto);
        public Task<bool> UpdateOfficeAsync(OfficeRequestDto requestDto);
        public Task UpdateOfficeBalanceAsync(long id, decimal addedAmount);
        public Task<OfficeViewModel> GetOfficeByID(long ID);
        public Task<OfficeViewModel> GetOfficeByName(string name);
        public Task<ICollection<OfficeViewModel>> GetOffices(string search, bool viewAll);

    }
}
