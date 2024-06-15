using AutoMapper;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO.Company;
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

        public CompanyRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<ICollection<CompanyViewModel>> GetCompanies()
        {
            var result = await _dataContext.companies.ToListAsync();
            return _mapper.Map<ICollection<CompanyViewModel>>(result);
        }

        public async Task<Company> GetCompanyByID(int id)
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

        public async Task<CompanyViewModel> RegisterAsync(CompanyRequestDto requestDto)
        {
            Company company = new Company
            {
                id = (_dataContext.companies.Max(x => (int?)x.id) ?? 0) + 1,
                name = requestDto.name,
                address = requestDto.address,
                contactno = requestDto.contactno,
                planid = requestDto.planid,
                isactive = false,
                isverified = false
            };
            _dataContext.companies.Add(company);
            Employee employee = new Employee
            {
                companyid = company.id,
                firstname = requestDto.adminfirstname,
                lastname = requestDto.adminlastname,
                email = requestDto.email,
                isactive = true,
                username = requestDto.email,
                userpass = Utility.GetHash("password"),
                isappsysadmin = true,
                datecreated = DateTime.Now
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
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}
