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

        public BankAccount GetBankAccountByID(long id)
        {
            return _dataContext.bankAccounts.Include(x => x.bank).Where(x => x.accountid == id).FirstOrDefault();
        }


        public Office CreateOffice(Office Office)
        {
            Office.accountid = 0;
            Office.datecreated = DateTime.Now;
            _dataContext.offices.Add(Office);
            _dataContext.SaveChanges();
            return Office;
        }

        public Office GetOfficeByID(long ID)
        {
            return _dataContext.offices.FirstOrDefault(x => x.accountid == ID);
        }

        public Office GetOfficeByName(int companyID, string name)
        {
            return _dataContext.offices.FirstOrDefault(x => x.companyid == companyID && x.accountname == name);
        }

        public async Task<ICollection<Office>> GetOffices(int companyID, string search, bool viewAll)
        {
            return await _dataContext.offices.Where(x => x.companyid == companyID && (x.isactive || viewAll) && x.accountname.Contains(search))
                .OrderBy(x => x.accountname).ToListAsync();
        }

        public Office UpdateOffice(OfficeRequestDto requestDto)
        {
            var Office = GetOfficeByID(requestDto.accountid);
            if (Office != null)
            {
                Office.accountname = requestDto.accountname;
                Office.isactive = requestDto.isactive;
                Office.memo = requestDto.memo;
                Office.updatedbyid = requestDto.updatedbyid;
                Office.dateupdated = DateTime.Now;
                _dataContext.SaveChanges();
            }
            return Office;
        }

        public void UpdateOfficeBalance(long id, decimal addedAmount)
        {
            var Office = GetOfficeByID(id);
            if (Office != null)
            {
                Office.pettycashbalance += addedAmount;
                _dataContext.SaveChanges();
            }
        }
    }
}
