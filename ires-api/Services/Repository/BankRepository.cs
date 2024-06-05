using AutoMapper;
using ires_api.Data;
using ires_api.DTO.Bank;
using ires_api.Models;
using ires_api.Services.Interface;
using ires_api.Services.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
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
        public async Task<Bank> Create(BankRequestDto requestDto)
        {
            Bank bank = _mapper.Map<Bank>(requestDto);
            bank.bankid = 0;
            _dataContext.banks.Add(bank);
            await _dataContext.SaveChangesAsync();
            return bank;
        }

        public async Task<ICollection<Bank>> GetAllBanks(int companyID, string search)
        {
            var banks = _dataContext.banks.Where(x => x.companyid == companyID && x.name.Contains(search)).OrderBy(x => x.name).ToList();
            if (banks.Count() == 0 && search == "")
            {
                BankSeeder bankSeeder = new BankSeeder(_dataContext);
                await bankSeeder.Seed(companyID, true);
                await bankSeeder.Seed(companyID, false);
                return await GetAllBanks(companyID, search);
            }
            return banks;
        }

        public async Task<Bank> GetBankByID(long bankID)
        {
            return await _dataContext.banks.FindAsync(bankID);
        }

        public async Task<Bank> GetBankByName(int companyID, string name)
        {
            return await _dataContext.banks.Where(x => x.companyid == companyID && x.name == name).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Bank>> GetBanks(int companyID, bool isEWallet, string search)
        {
            var banks = _dataContext.banks.Where(x => x.companyid == companyID && x.isewallet == isEWallet && x.name.Contains(search)).ToList();
            if (banks.Count() == 0 && search == "")
            {
                BankSeeder bankSeeder = new BankSeeder(_dataContext);
                await bankSeeder.Seed(companyID, isEWallet);
                return await GetBanks(companyID, isEWallet, search);
            }
            return banks;
        }

        public async Task<Bank> Update(BankRequestDto requestDto)
        {
            Bank bank = await GetBankByID(requestDto.bankid);
            if (bank != null)
            {
                bank.name = requestDto.name;
                bank.isewallet = requestDto.isewallet;
                await _dataContext.SaveChangesAsync();
            }
            return bank;
        }
    }
}
