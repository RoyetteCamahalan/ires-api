using AutoMapper;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.BankAccount;
using ires.Domain.DTO.Office;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Infrastructure.Extensions;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class AccountRepository(
        DataContext _dataContext,
        ICurrentUserService _currentUserService,
        IMapper _mapper,
        ILogService _logService) : IAccountService
    {

        private async Task<BankAccount> GetBankAccount(long id)
        {
            return await _dataContext.bankAccounts.Include(x => x.bank).Where(x => x.accountid == id).FirstOrDefaultAsync() ??
                throw new EntityNotFoundException();
        }

        private async Task<Office> GetOffice(long ID)
        {
            return await _dataContext.offices.FirstOrDefaultAsync(x => x.accountid == ID) ??
                throw new EntityNotFoundException();
        }

        public async Task UpdateOfficeBalanceAsync(long id, decimal addedAmount)
        {
            var Office = await GetOffice(id);
            Office.pettycashbalance += addedAmount;
            await _dataContext.SaveChangesAsync();
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
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.bankAccounts.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.BankAccounts, "Create New Bank Account", "Account : " + entity.accountid + '-' + entity.accountname);
            return _mapper.Map<BankAccountViewModel>(entity);
        }

        public async Task UpdateBankAccountAsync(BankAccountRequestDto requestDto)
        {
            var entity = await GetBankAccount(requestDto.accountid);
            entity.accountname = requestDto.accountname;
            entity.accountno = requestDto.accountno;
            entity.bankid = requestDto.bankid;
            entity.bankpreferredbranch = requestDto.bankpreferredbranch;
            entity.isactive = requestDto.isactive;
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.BankAccounts, "Updated Bank Account", "Account ID: " + entity.accountid + '-' + entity.accountname);
        }

        public async Task<PaginatedResult<BankAccountViewModel>> GetBankAccounts(PaginationRequest request)
        {
            var query = _dataContext.bankAccounts.Include(x => x.bank).Where(x => x.companyid == _currentUserService.companyid &&
                (x.accountname.Contains(request.searchString) || x.accountno.Contains(request.searchString) || (x.bank.name ?? "").Contains(request.searchString)))
                .OrderBy(x => x.accountname).AsQueryable();
            return await query.AsPaginatedResult<BankAccount, BankAccountViewModel>(request, _mapper.ConfigurationProvider);
        }

        public async Task<bool> isBankAccountExist(BankAccountRequestDto requestDto)
        {
            return await _dataContext.bankAccounts.Where(x => x.companyid == _currentUserService.companyid && x.bankid == requestDto.bankid
                && x.accountname == requestDto.accountname).AnyAsync();
        }

        public async Task<OfficeViewModel> CreateOfficeAsync(OfficeRequestDto requestDto)
        {
            var entity = _mapper.Map<Office>(requestDto);
            entity.accountid = 0;
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.offices.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Offices, "Office", "Create New Office : " + entity.accountid + "-" + entity.accountname);
            return _mapper.Map<OfficeViewModel>(entity);
        }

        public async Task UpdateOfficeAsync(OfficeRequestDto requestDto)
        {
            var entity = await GetOffice(requestDto.accountid);
            entity.accountname = requestDto.accountname;
            entity.isactive = requestDto.isactive;
            entity.memo = requestDto.memo;
            entity.updatedbyid = _currentUserService.employeeid;
            entity.dateupdated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Offices, "Office", "Update Office ID : " + requestDto.accountid);
        }

        public async Task<OfficeViewModel> GetOfficeByID(long ID)
        {
            var result = await GetOffice(ID);
            return _mapper.Map<OfficeViewModel>(result);
        }

        public async Task<OfficeViewModel> GetOfficeByName(string name)
        {
            var result = await _dataContext.offices.FirstOrDefaultAsync(x => x.companyid == _currentUserService.companyid && x.accountname == name);
            return _mapper.Map<OfficeViewModel>(result);
        }

        public async Task<PaginatedResult<OfficeViewModel>> GetOffices(PaginationRequest request)
        {
            var query = _dataContext.offices.Where(x => x.companyid == _currentUserService.companyid &&
                (x.isactive || request.viewAll) && x.accountname.Contains(request.searchString))
                .OrderBy(x => x.accountname).AsQueryable();
            return await query.AsPaginatedResult<Office, OfficeViewModel>(request, _mapper.ConfigurationProvider);
        }
    }
}
