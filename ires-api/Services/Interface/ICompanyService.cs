using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface ICompanyService
    {
        public Task<ICollection<Company>> GetCompanies();
        public Task<Company> GetCompanyByID(int id);
        public Task<Company> GetCompanyByName(string name);
        public Task<Company> RegisterAsync(CompanyRequestDto requestDto);
        public Task<bool> Verify(int id);
    }
}
