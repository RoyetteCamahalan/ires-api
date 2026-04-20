using AutoMapper;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO.BankAccount;
using ires.Domain.DTO.Office;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace ires.Infrastructure.Repositories
{
    public class AccountRepository(
        DataContext dataContext, 
        IMapper mapper, 
        ICurrentUserContext currentUserContext, 
        ILogService logService) : IAccountService
    {
        private async Task<BankAccount> GetBankAccount(long id)
        {
            return await dataContext.bankAccounts.Include(x => x.bank).Where(x => x.accountid == id).FirstOrDefaultAsync();
        }

        private async Task<Office> GetOffice(long ID)
        {
            return await dataContext.offices.FirstOrDefaultAsync(x => x.accountid == ID);
        }

        public async Task UpdateOfficeBalanceAsync(long id, decimal addedAmount)
        {
            var Office = await GetOffice(id);
            if (Office != null)
            {
                Office.pettycashbalance += addedAmount;
                await dataContext.SaveChangesAsync();
            }
        }

        public async Task<BankAccountViewModel> GetBankAccountByID(long id)
        {
            var result = await GetBankAccount(id);
            return mapper.Map<BankAccountViewModel>(result);
        }

        public async Task<BankAccountViewModel> CreateBankAccountAsync(BankAccountRequestDto requestDto)
        {
            var entity = mapper.Map<BankAccount>(requestDto);
            entity.accountid = 0;
            entity.datecreated = Utility.GetServerTime();
            dataContext.bankAccounts.Add(entity);
            await dataContext.SaveChangesAsync();
            await logService.SaveLogAsync(entity.companyid, entity.createdbyid, AppModule.BankAccounts, "Create New Bank Account", "Account : " + entity.accountid + '-' + entity.accountname, 0);
            return mapper.Map<BankAccountViewModel>(entity);
        }

        public async Task<bool> UpdateBankAccountAsync(BankAccountRequestDto requestDto)
        {
            var bankAccount = await GetBankAccount(requestDto.accountid);
            if (bankAccount != null)
            {
                bankAccount.accountname = requestDto.accountname;
                bankAccount.accountno = requestDto.accountno;
                bankAccount.bankid = requestDto.bankid;
                bankAccount.bankpreferredbranch = requestDto.bankpreferredbranch;
                bankAccount.isactive = requestDto.isactive;
                await dataContext.SaveChangesAsync();
                await logService.SaveLogAsync(bankAccount.companyid, requestDto.updatedbyid, AppModule.BankAccounts, "Updated Bank Account", "Account ID: " + bankAccount.accountid + '-' + bankAccount.accountname, 0);
                return true;
            }
            return false;
        }

        public async Task<ICollection<BankAccountViewModel>> GetBankAccounts(string search)
        {
            var result = await dataContext.bankAccounts.Include(x => x.bank).Where(x => x.companyid == currentUserContext.companyid &&
                (x.accountname.Contains(search) || x.accountno.Contains(search) || (x.bank.name ?? "").Contains(search)))
                .OrderByDescending(x => x.accountname).ToListAsync();
            return mapper.Map<ICollection<BankAccountViewModel>>(result);
        }

        public async Task<bool> isBankAccountExist(BankAccountRequestDto requestDto)
        {
            return await dataContext.bankAccounts.Where(x => x.companyid == requestDto.companyid && x.bankid == requestDto.bankid && x.accountname == requestDto.accountname).AnyAsync();
        }

        public async Task<OfficeViewModel> CreateOfficeAsync(OfficeRequestDto requestDto)
        {
            var entity = mapper.Map<Office>(requestDto);
            entity.accountid = 0;
            entity.companyid = currentUserContext.companyid;
            entity.createdbyid = currentUserContext.employeeid;
            entity.datecreated = Utility.GetServerTime();
            dataContext.offices.Add(entity);
            await dataContext.SaveChangesAsync();
            await logService.SaveLogAsync(entity.companyid, entity.createdbyid, AppModule.Offices, "Office", "Create New Office : " + entity.accountid + "-" + entity.accountname, 0);
            return mapper.Map<OfficeViewModel>(entity);
        }

        public async Task<bool> UpdateOfficeAsync(OfficeRequestDto requestDto)
        {
            var entity = await GetOffice(requestDto.accountid);
            if (entity != null)
            {
                entity.accountname = requestDto.accountname;
                entity.isactive = requestDto.isactive;
                entity.memo = requestDto.memo;
                entity.updatedbyid = currentUserContext.employeeid;
                entity.dateupdated = Utility.GetServerTime();
                await dataContext.SaveChangesAsync();
                await logService.SaveLogAsync(entity.companyid, entity.updatedbyid, 0, "Office", "Update Office ID : " + requestDto.accountid, 0);
                return true;
            }
            return false;
        }

        public async Task<OfficeViewModel> GetOfficeByID(long ID)
        {
            var result = await GetOffice(ID);
            return mapper.Map<OfficeViewModel>(result);
        }

        public async Task<OfficeViewModel> GetOfficeByName(string name)
        {
            var result = await dataContext.offices.FirstOrDefaultAsync(x => x.companyid == currentUserContext.companyid && x.accountname == name);
            return mapper.Map<OfficeViewModel>(result);
        }

        public async Task<ICollection<OfficeViewModel>> GetOffices(string search, bool viewAll)
        {
            var result = await dataContext.offices.Where(x => x.companyid == currentUserContext.companyid && (x.isactive || viewAll) && x.accountname.Contains(search))
                .OrderBy(x => x.accountname).ToListAsync();
            return mapper.Map<ICollection<OfficeViewModel>>(result);
        }
    }
}
