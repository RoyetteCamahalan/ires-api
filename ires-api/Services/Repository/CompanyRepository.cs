using ires_api.Data;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;

namespace ires_api.Services.Repository
{
    public class CompanyRepository : ICompanyService
    {
        private readonly DataContext _dataContext;

        public CompanyRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public ICollection<Company> GetCompanies()
        {
            return _dataContext.companies.ToList();
        }

        public Company GetCompanyByID(int id)
        {
            return _dataContext.companies.Find(id);
        }

        public Company GetCompanyByName(string name)
        {
            return _dataContext.companies.Where(x => x.name == name).FirstOrDefault();
        }

        public Company Register(CompanyRequestDto requestDto)
        {
            Company company = new Company
            {
                id = (_dataContext.companies.Max(x => (int?)x.id) ?? 0) + 1,
                name = requestDto.name,
                address = requestDto.address,
                contactno = requestDto.contactno,
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
            _dataContext.SaveChanges();
            return company;
        }

        public bool Verify(int id)
        {
            var company = GetCompanyByID(id);
            if (company == null)
                return false;
            company.isverified = true;
            _dataContext.SaveChanges();
            return true;
        }
    }
}
