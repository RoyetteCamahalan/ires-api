using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IBankService
    {
        public Bank Create(BankRequestDto requestDto);
        public Bank Update(BankRequestDto requestDto);
        public Bank GetBankByID(long bankID);
        public Bank GetBankByName(int companyID, string name);
        public ICollection<Bank> GetBanks(int companyID, bool isEWallet, string search);
        public ICollection<Bank> GetAllBanks(int companyID, string search);
    }
}
