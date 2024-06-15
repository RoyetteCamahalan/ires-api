using AutoMapper;
using ires.Domain.Contracts;
using ires.Domain.DTO.BankAccount;
using ires.Domain.DTO.Office;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class AccountRepository : IAccountService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public AccountRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        private async Task<BankAccount> GetBankAccount(long id)
        {
            return await _dataContext.bankAccounts.Include(x => x.bank).Where(x => x.accountid == id).FirstOrDefaultAsync();
        }

        private async Task<Office> GetOffice(long ID)
        {
            return await _dataContext.offices.FirstOrDefaultAsync(x => x.accountid == ID);
        }

        public async Task UpdateOfficeBalanceAsync(long id, decimal addedAmount)
        {
            var Office = await GetOffice(id);
            if (Office != null)
            {
                Office.pettycashbalance += addedAmount;
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<BankAccountViewModel> GetBankAccountByID(long id)
        {
            var result = await GetBankAccount(id);
            return _mapper.Map<BankAccountViewModel>(result);
        }

        public async Task<BankAccountViewModel> CreateBankAccountAsync(BankAccountRequestDto requestDto)
        {
            var entity = _mapper.Map<BankAccount>(requestDto);
            entity.accountid = 0;
            entity.datecreated = DateTime.Now;
            _dataContext.bankAccounts.Add(entity);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<BankAccountViewModel>(entity);
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

                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ICollection<BankAccountViewModel>> GetBankAccounts(int companyID, string search)
        {
            var result = await _dataContext.bankAccounts.Include(x => x.bank).Where(x => x.companyid == companyID &&
                (x.accountname.Contains(search) || x.accountno.Contains(search) || (x.bank.name ?? "").Contains(search)))
                .OrderByDescending(x => x.accountname).ToListAsync();
            return _mapper.Map<ICollection<BankAccountViewModel>>(result);
        }

        public async Task<bool> isBankAccountExist(BankAccountRequestDto requestDto)
        {
            return await _dataContext.bankAccounts.Where(x => x.companyid == requestDto.companyid && x.bankid == requestDto.bankid && x.accountname == requestDto.accountname).AnyAsync();
        }

        public async Task<OfficeViewModel> CreateOfficeAsync(OfficeRequestDto requestDto)
        {
            var entity = _mapper.Map<Office>(requestDto);
            entity.accountid = 0;
            entity.datecreated = DateTime.Now;
            _dataContext.offices.Add(entity);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<OfficeViewModel>(entity);
        }

        public async Task<bool> UpdateOfficeAsync(OfficeRequestDto requestDto)
        {
            var Office = await GetOffice(requestDto.accountid);
            if (Office != null)
            {
                Office.accountname = requestDto.accountname;
                Office.isactive = requestDto.isactive;
                Office.memo = requestDto.memo;
                Office.updatedbyid = requestDto.updatedbyid;
                Office.dateupdated = DateTime.Now;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<OfficeViewModel> GetOfficeByID(long ID)
        {
            var result = await GetOffice(ID);
            return _mapper.Map<OfficeViewModel>(result);
        }

        public async Task<OfficeViewModel> GetOfficeByName(int companyID, string name)
        {
            var result = await _dataContext.offices.FirstOrDefaultAsync(x => x.companyid == companyID && x.accountname == name);
            return _mapper.Map<OfficeViewModel>(result);
        }

        public async Task<ICollection<OfficeViewModel>> GetOffices(int companyID, string search, bool viewAll)
        {
            var result = await _dataContext.offices.Where(x => x.companyid == companyID && (x.isactive || viewAll) && x.accountname.Contains(search))
                .OrderBy(x => x.accountname).ToListAsync();
            return _mapper.Map<ICollection<OfficeViewModel>>(result);
        }
    }
}
