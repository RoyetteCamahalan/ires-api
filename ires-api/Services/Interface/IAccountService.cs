using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IAccountService
    {
        public BankAccount GetBankAccountByID(long id);
        public BankAccount CreateBankAccount(BankAccount requestDto);
        public BankAccount UpdateBankAccount(BankAccountRequestDto requestDto);
        public ICollection<BankAccount> GetBankAccounts(int companyID, string search);
        public bool isBankAccountExist(BankAccountRequestDto requestDto);

    }
}
