using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface ICompanyService
    {
        public ICollection<Company> GetCompanies();
        public Company GetCompanyByID(int id);
        public Company GetCompanyByName(string name);
        public Company Register(CompanyRequestDto requestDto);
        public bool Verify(int id);
    }
}
