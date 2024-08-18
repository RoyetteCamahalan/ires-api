using ires.Domain.Common;
using ires.Domain.DTO.BankAccount;
using ires.Domain.DTO.Office;

namespace ires.Domain.Contracts
{
    public interface IAccountService
    {
        public Task<BankAccountViewModel> GetBankAccountByID(long id);
        public Task<BankAccountViewModel> CreateBankAccountAsync(BankAccountRequestDto requestDto);
        public Task UpdateBankAccountAsync(BankAccountRequestDto requestDto);
        public Task<PaginatedResult<BankAccountViewModel>> GetBankAccounts(PaginationRequest request);
        public Task<bool> isBankAccountExist(BankAccountRequestDto requestDto);

        public Task<OfficeViewModel> CreateOfficeAsync(OfficeRequestDto requestDto);
        public Task UpdateOfficeAsync(OfficeRequestDto requestDto);
        public Task UpdateOfficeBalanceAsync(long id, decimal addedAmount);
        public Task<OfficeViewModel> GetOfficeByID(long ID);
        public Task<OfficeViewModel> GetOfficeByName(string name);
        public Task<PaginatedResult<OfficeViewModel>> GetOffices(PaginationRequest request);

    }
}
