using AutoMapper;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO.Company;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using ires.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class CompanyRepository(
        DataContext _dataContext,
        IMapper _mapper,
        ILogService _logService,
        ICurrentUserService _currentUserService) : ICompanyService
    {

        public async Task<ICollection<CompanyViewModel>> GetCompanies()
        {
            var result = await _dataContext.companies.ToListAsync();
            return _mapper.Map<ICollection<CompanyViewModel>>(result);
        }

        private async Task<Company> GetCompanyByID(int id)
        {
            return await _dataContext.companies.FindAsync(id) ?? throw new EntityNotFoundException();
        }

        public async Task<CompanyViewModel> GetByID()
        {
            var result = await GetCompanyByID(_currentUserService.companyid);
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

        public async Task Verify(int id)
        {
            var company = await GetCompanyByID(id);
            company.isverified = true;
            company.isactive = true;
            await _dataContext.SaveChangesAsync();
        }

        public async Task CompleteTour(int id)
        {
            var company = await GetCompanyByID(id);
            company.apptour = 100;
            await _dataContext.SaveChangesAsync();
        }

        public async Task Update(UpdateCompanyRequestDto requestDto)
        {
            var entity = await GetCompanyByID(_currentUserService.companyid);
            entity.name = requestDto.name;
            entity.contactno = requestDto.contactno;
            entity.address = requestDto.address;
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Company, "Updated Company Info", "Updated Company Info", 1);
        }
    }
}
