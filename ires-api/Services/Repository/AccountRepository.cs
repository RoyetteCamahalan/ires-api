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

        public async Task<BankAccount> CreateBankAccountAsync(BankAccount requestDto)
        {
            requestDto.accountid = 0;
            requestDto.datecreated = DateTime.Now;
            _dataContext.bankAccounts.Add(requestDto);
            await _dataContext.SaveChangesAsync();
            return requestDto;
        }

        public async Task<ICollection<BankAccount>> GetBankAccounts(int companyID, string search)
        {
            return await _dataContext.bankAccounts.Include(x => x.bank).Where(x => x.companyid == companyID &&
                (x.accountname.Contains(search) || x.accountno.Contains(search) || (x.bank.name ?? "").Contains(search)))
                .OrderByDescending(x => x.accountname).ToListAsync();
        }

        public async Task<bool> isBankAccountExist(BankAccountRequestDto requestDto)
        {
            return await _dataContext.bankAccounts.Where(x => x.companyid == requestDto.companyid && x.bankid == requestDto.bankid && x.accountname == requestDto.accountname).CountAsync() > 0;
        }

        public async Task<BankAccount> UpdateBankAccountAsync(BankAccountRequestDto requestDto)
        {
            BankAccount bankAccount = await GetBankAccountByID(requestDto.accountid);
            if (bankAccount != null)
            {
                bankAccount.accountname = requestDto.accountname;
                bankAccount.accountno = requestDto.accountno;
                bankAccount.bankid = requestDto.bankid;
                bankAccount.bankpreferredbranch = requestDto.bankpreferredbranch;
                bankAccount.isactive = requestDto.isactive;

                await _dataContext.SaveChangesAsync();
            }
            return bankAccount;
        }

        public async Task<BankAccount> GetBankAccountByID(long id)
        {
            return await _dataContext.bankAccounts.Include(x => x.bank).Where(x => x.accountid == id).FirstOrDefaultAsync();
        }


        public async Task<Office> CreateOfficeAsync(Office Office)
        {
            Office.accountid = 0;
            Office.datecreated = DateTime.Now;
            _dataContext.offices.Add(Office);
            await _dataContext.SaveChangesAsync();
            return Office;
        }

        public async Task<Office> GetOfficeByID(long ID)
        {
            return await _dataContext.offices.FirstOrDefaultAsync(x => x.accountid == ID);
        }

        public async Task<Office> GetOfficeByName(int companyID, string name)
        {
            return await _dataContext.offices.FirstOrDefaultAsync(x => x.companyid == companyID && x.accountname == name);
        }

        public async Task<ICollection<Office>> GetOffices(int companyID, string search, bool viewAll)
        {
            return await _dataContext.offices.Where(x => x.companyid == companyID && (x.isactive || viewAll) && x.accountname.Contains(search))
                .OrderBy(x => x.accountname).ToListAsync();
        }

        public async Task<Office> UpdateOfficeAsync(OfficeRequestDto requestDto)
        {
            var Office = await GetOfficeByID(requestDto.accountid);
            if (Office != null)
            {
                Office.accountname = requestDto.accountname;
                Office.isactive = requestDto.isactive;
                Office.memo = requestDto.memo;
                Office.updatedbyid = requestDto.updatedbyid;
                Office.dateupdated = DateTime.Now;
                await _dataContext.SaveChangesAsync();
            }
            return Office;
        }

        public async Task UpdateOfficeBalanceAsync(long id, decimal addedAmount)
        {
            var Office = await GetOfficeByID(id);
            if (Office != null)
            {
                Office.pettycashbalance += addedAmount;
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
