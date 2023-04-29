using AutoMapper;
using ires_api.Data;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using ires_api.Services.Seeders;

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
        public Bank Create(BankRequestDto requestDto)
        {
            Bank bank = _mapper.Map<Bank>(requestDto);
            bank.bankid = 0;
            _dataContext.banks.Add(bank);
            _dataContext.SaveChanges();
            return bank;
        }

        public ICollection<Bank> GetAllBanks(int companyID, string search)
        {
            var banks = _dataContext.banks.Where(x => x.companyid == companyID && x.name.Contains(search)).OrderBy(x => x.name).ToList();
            if (banks.Count() == 0 && search == "")
            {
                BankSeeder bankSeeder = new BankSeeder(_dataContext);
                bankSeeder.Seed(companyID, true);
                bankSeeder.Seed(companyID, false);
                return GetAllBanks(companyID, search);
            }
            return banks;
        }

        public Bank GetBankByID(long bankID)
        {
            return _dataContext.banks.Find(bankID);
        }

        public Bank GetBankByName(int companyID, string name)
        {
            return _dataContext.banks.Where(x => x.companyid == companyID && x.name == name).FirstOrDefault();
        }

        public ICollection<Bank> GetBanks(int companyID, bool isEWallet, string search)
        {
            var banks = _dataContext.banks.Where(x => x.companyid == companyID && x.isewallet == isEWallet && x.name.Contains(search)).ToList();
            if (banks.Count() == 0 && search == "")
            {
                BankSeeder bankSeeder = new BankSeeder(_dataContext);
                bankSeeder.Seed(companyID, isEWallet);
                return GetBanks(companyID, isEWallet, search);
            }
            return banks;
        }

        public Bank Update(BankRequestDto requestDto)
        {
            Bank bank = GetBankByID(requestDto.bankid);
            if (bank != null)
            {
                bank.name = requestDto.name;
                bank.isewallet = requestDto.isewallet;
                _dataContext.SaveChanges();
            }
            return bank;
        }
    }
}
