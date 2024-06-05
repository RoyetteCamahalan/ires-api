using ires_api.DTO.Bank;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IBankService
    {
        public Task<Bank> Create(BankRequestDto requestDto);
        public Task<Bank> Update(BankRequestDto requestDto);
        public Task<Bank> GetBankByID(long bankID);
        public Task<Bank> GetBankByName(int companyID, string name);
        public Task<ICollection<Bank>> GetBanks(int companyID, bool isEWallet, string search);
        public Task<ICollection<Bank>> GetAllBanks(int companyID, string search);
    }
}
