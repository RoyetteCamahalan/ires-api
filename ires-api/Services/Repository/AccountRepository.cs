using ires_api.Data;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
{
    public class AccountRepository : IAccountService
    {
        private readonly DataContext _dataContext;

        public AccountRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public BankAccount CreateBankAccount(BankAccount requestDto)
        {
            requestDto.accountid = 0;
            requestDto.datecreated = DateTime.Now;
            _dataContext.bankAccounts.Add(requestDto);
            _dataContext.SaveChanges();
            return requestDto;
        }

        public BankAccount GetBankAccountByID(long id)
        {
            return _dataContext.bankAccounts.Find(id);
        }

        public ICollection<BankAccount> GetBankAccounts(int companyID, string search)
        {
            return _dataContext.bankAccounts.Include(x => x.bank).Where(x => x.companyid == companyID &&
                (x.accountname.Contains(search) || x.accountno.Contains(search) || (x.bank.name ?? "").Contains(search)))
                .OrderByDescending(x => x.accountname).ToList();
        }

        public bool isBankAccountExist(BankAccountRequestDto requestDto)
        {
            return _dataContext.bankAccounts.Where(x => x.companyid == requestDto.companyid && x.bankid == requestDto.bankid && x.accountname == requestDto.accountname).Count() > 0;
        }

        public BankAccount UpdateBankAccount(BankAccountRequestDto requestDto)
        {
            BankAccount bankAccount = GetBankAccountByID(requestDto.accountid);
            if (bankAccount != null)
            {
                bankAccount.accountname = requestDto.accountname;
                bankAccount.accountno = requestDto.accountno;
                bankAccount.bankid = requestDto.bankid;
                bankAccount.bankpreferredbranch = requestDto.bankpreferredbranch;
                bankAccount.isactive = requestDto.isactive;

                _dataContext.SaveChanges();
            }
            return bankAccount;
        }
    }
}
