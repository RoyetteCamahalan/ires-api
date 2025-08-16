using AutoMapper;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO.Company;
using ires.Domain.DTO.CompanySetting;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using ires.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogService _logService;

        public CompanyRepository(DataContext dataContext, IMapper mapper, 
            ICurrentUserService currentUserService, ILogService logService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _logService = logService;
        }
        public async Task<ICollection<CompanyViewModel>> GetCompanies()
        {
            var result = await _dataContext.companies.ToListAsync();
            return _mapper.Map<ICollection<CompanyViewModel>>(result);
        }

        private async Task<Company> GetCompanyByID(int id)
        {
            return await _dataContext.companies.FindAsync(id);
        }

        public async Task<CompanyViewModel> GetByID(int id)
        {
            var result = await GetCompanyByID(id);
            return _mapper.Map<CompanyViewModel>(result);
        }

        public async Task<CompanyViewModel> GetCompanyByName(string name)
        {
            var result = await _dataContext.companies.Where(x => x.name == name).FirstOrDefaultAsync();
            return _mapper.Map<CompanyViewModel>(result);
        }

        public async Task<CompanyViewModel> RegisterAsync(RegisterCompanyRequestDto requestDto)
        {
            var plan = await _dataContext.subscriptionPlans.FindAsync(requestDto.planid);
            Company company = new Company
            {
                name = requestDto.name,
                address = requestDto.address,
                contactno = requestDto.contactno,
                email = requestDto.email,
                planid = requestDto.planid,
                subscriptionexpiry = Utility.GetServerTime().AddMonths(1),
                surveylimit = plan.surveylimit,
                isactive = false,
                isverified = false,
                billingcycle = Domain.Enumerations.BillingCycle.monthly
            };
            _dataContext.companies.Add(company);
            Employee employee = new Employee
            {
                company = company,
                firstname = requestDto.adminfirstname,
                lastname = requestDto.adminlastname,
                email = requestDto.email,
                isactive = true,
                username = requestDto.email,
                userpass = Utility.GetHash(requestDto.password),
                isappsysadmin = true,
                datecreated = Utility.GetServerTime()
            };
            _dataContext.employees.Add(employee);
            await _dataContext.SaveChangesAsync();
            BankSeeder bankSeeder = new BankSeeder(_dataContext);
            await bankSeeder.Seed(company.id, false);
            await bankSeeder.Seed(company.id, true);
            return _mapper.Map<CompanyViewModel>(company);
        }

        public async Task<bool> Verify(int id)
        {
            var company = await GetCompanyByID(id);
            if (company == null)
                return false;
            company.isverified = true;
            company.isactive = true;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteTour(int id)
        {
            var company = await GetCompanyByID(id);
            if (company == null)
                return false;
            company.apptour = 100;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(UpdateCompanyRequestDto requestDto)
        {
            var entity = await GetCompanyByID(requestDto.id);
            if (entity != null)
            {
                entity.name = requestDto.name;
                entity.contactno = requestDto.contactno;
                entity.address = requestDto.address;
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(entity.id, requestDto.updatedbyid, Domain.Enumerations.AppModule.Company, "Updated Company Info", "Updated Company Info", 1);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateSettings(CompanySettingDto requestDto)
        {
            var settings = await _dataContext.companySettings.Where(x => x.companyid == _currentUserService.companyid).FirstOrDefaultAsync();
            if (settings != null)
            {
                settings.autocashinaccountid_survey = requestDto.autocashinaccountid_survey;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<CompanySettingViewModel> GetSettings()
        {
            var settings = await _dataContext.companySettings.Where(x => x.companyid == _currentUserService.companyid).FirstOrDefaultAsync();
            if(settings == null)
            {
                settings = new CompanySetting
                {
                    companyid = _currentUserService.companyid,
                    autocashinaccountid_survey = null
                };
                _dataContext.companySettings.Add(settings);
                await _dataContext.SaveChangesAsync();
            }
            return _mapper.Map<CompanySettingViewModel>(settings);
        }
    }
}
