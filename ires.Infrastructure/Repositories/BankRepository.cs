using AutoMapper;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.Bank;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Infrastructure.Extensions;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using ires.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class BankRepository(
        DataContext _dataContext,
        IMapper _mapper,
        ILogService _logService,
        ICurrentUserService _currentUserService) : IBankService
    {

        public async Task<BankViewModel> Create(BankRequestDto requestDto)
        {
            var entity = _mapper.Map<Bank>(requestDto);
            entity.bankid = 0;
            entity.companyid = _currentUserService.companyid;
            _dataContext.banks.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Banks, "Create New Bank", "Bank ID : " + entity.bankid + "-" + entity.name);
            return _mapper.Map<BankViewModel>(entity);
        }

        public async Task<PaginatedResult<BankViewModel>> GetAllBanks(PaginationRequest request)
        {
            var query = _dataContext.banks.Where(x => x.companyid == _currentUserService.companyid && x.name.Contains(request.searchString)).OrderBy(x => x.name).AsQueryable();
            if (!query.Any() && request.search == "")
            {
                BankSeeder bankSeeder = new BankSeeder(_dataContext);
                await bankSeeder.Seed(_currentUserService.companyid, true);
                await bankSeeder.Seed(_currentUserService.companyid, false);
                return await GetAllBanks(request);
            }
            return await query.AsPaginatedResult<Bank, BankViewModel>(request, _mapper.ConfigurationProvider);
        }

        private async Task<Bank> GetBank(long bankID)
        {
            return await _dataContext.banks.FindAsync(bankID) ?? throw new EntityNotFoundException();
        }

        public async Task<BankViewModel> GetBankByID(long bankID)
        {
            var result = await GetBank(bankID);
            return _mapper.Map<BankViewModel>(result);
        }

        public async Task<BankViewModel> GetBankByName(string name)
        {
            var result = await _dataContext.banks.Where(x => x.companyid == _currentUserService.companyid && x.name == name).FirstOrDefaultAsync();
            return _mapper.Map<BankViewModel>(result);
        }

        public async Task<PaginatedResult<BankViewModel>> GetBanks(PaginationRequest request)
        {
            var query = _dataContext.banks.Where(x => x.companyid == _currentUserService.companyid && x.isewallet == request.isEWallet &&
                x.name.Contains(request.searchString)).OrderBy(x => x.name).AsQueryable();
            if (!query.Any() && request.search == "")
            {
                BankSeeder bankSeeder = new BankSeeder(_dataContext);
                await bankSeeder.Seed(_currentUserService.companyid, request.isEWallet);
                return await GetBanks(request);
            }
            return await query.AsPaginatedResult<Bank, BankViewModel>(request, _mapper.ConfigurationProvider);
        }

        public async Task Update(BankRequestDto requestDto)
        {
            var entity = await GetBank(requestDto.bankid);
            entity.name = requestDto.name;
            entity.isewallet = requestDto.isewallet;
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Banks, "Update Bank", "Bank ID : " + entity.bankid + "-" + entity.name, 0);
        }
    }
}
