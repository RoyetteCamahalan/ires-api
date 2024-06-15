using ires.Domain.DTO.Bank;

namespace ires.Domain.Contracts
{
    public interface IBankService
    {
        public Task<BankViewModel> Create(BankRequestDto requestDto);
        public Task<bool> Update(BankRequestDto requestDto);
        public Task<BankViewModel> GetBankByID(long bankID);
        public Task<BankViewModel> GetBankByName(int companyID, string name);
        public Task<ICollection<BankViewModel>> GetBanks(int companyID, bool isEWallet, string search);
        public Task<ICollection<BankViewModel>> GetAllBanks(int companyID, string search);
    }
}
