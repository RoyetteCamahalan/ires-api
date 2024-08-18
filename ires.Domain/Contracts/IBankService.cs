using ires.Domain.Common;
using ires.Domain.DTO.Bank;

namespace ires.Domain.Contracts
{
    public interface IBankService
    {
        public Task<BankViewModel> Create(BankRequestDto requestDto);
        public Task Update(BankRequestDto requestDto);
        public Task<BankViewModel> GetBankByID(long bankID);
        public Task<BankViewModel> GetBankByName(string name);
        public Task<PaginatedResult<BankViewModel>> GetBanks(PaginationRequest request);
        public Task<PaginatedResult<BankViewModel>> GetAllBanks(PaginationRequest request);
    }
}
