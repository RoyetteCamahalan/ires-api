using AutoMapper;
using ires.Domain.Contracts;
using ires.Domain.DTO.Bank;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using ires.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class BankRepository : IBankService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public BankRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<BankViewModel> Create(BankRequestDto requestDto)
        {
            var entity = _mapper.Map<Bank>(requestDto);
            entity.bankid = 0;
            _dataContext.banks.Add(entity);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<BankViewModel>(entity);
        }

        public async Task<ICollection<BankViewModel>> GetAllBanks(int companyID, string search)
        {
            var banks = _dataContext.banks.Where(x => x.companyid == companyID && x.name.Contains(search)).OrderBy(x => x.name).ToList();
            if (banks.Count == 0 && search == "")
            {
                BankSeeder bankSeeder = new BankSeeder(_dataContext);
                await bankSeeder.Seed(companyID, true);
                await bankSeeder.Seed(companyID, false);
                return await GetAllBanks(companyID, search);
            }
            return _mapper.Map<ICollection<BankViewModel>>(banks);
        }

        private async Task<Bank> GetBank(long bankID)
        {
            return await _dataContext.banks.FindAsync(bankID);
        }

        public async Task<BankViewModel> GetBankByID(long bankID)
        {
            var result = await GetBank(bankID);
            return _mapper.Map<BankViewModel>(result);
        }

        public async Task<BankViewModel> GetBankByName(int companyID, string name)
        {
            var result = await _dataContext.banks.Where(x => x.companyid == companyID && x.name == name).FirstOrDefaultAsync();
            return _mapper.Map<BankViewModel>(result);
        }

        public async Task<ICollection<BankViewModel>> GetBanks(int companyID, bool isEWallet, string search)
        {
            var banks = _dataContext.banks.Where(x => x.companyid == companyID && x.isewallet == isEWallet && x.name.Contains(search)).ToList();
            if (banks.Count == 0 && search == "")
            {
                BankSeeder bankSeeder = new BankSeeder(_dataContext);
                await bankSeeder.Seed(companyID, isEWallet);
                return await GetBanks(companyID, isEWallet, search);
            }
            return _mapper.Map<ICollection<BankViewModel>>(banks);
        }

        public async Task<bool> Update(BankRequestDto requestDto)
        {
            var bank = await GetBank(requestDto.bankid);
            if (bank != null)
            {
                bank.name = requestDto.name;
                bank.isewallet = requestDto.isewallet;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
