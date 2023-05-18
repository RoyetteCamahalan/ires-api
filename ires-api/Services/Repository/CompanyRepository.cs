using ires_api.Data;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using ires_api.Services.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
{
    public class CompanyRepository : ICompanyService
    {
        private readonly DataContext _dataContext;

        public CompanyRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<ICollection<Company>> GetCompanies()
        {
            return await _dataContext.companies.ToListAsync();
        }

        public async Task<Company> GetCompanyByID(int id)
        {
            return await _dataContext.companies.FindAsync(id);
        }

        public async Task<Company> GetCompanyByName(string name)
        {
            return await _dataContext.companies.Where(x => x.name == name).FirstOrDefaultAsync();
        }

        public async Task<Company> RegisterAsync(CompanyRequestDto requestDto)
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
            return company;
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
