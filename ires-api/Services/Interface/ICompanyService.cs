using ires_api.DTO.Company;

namespace ires_api.Services.Interface
{
    public interface ICompanyService
    {
        public Task<ICollection<CompanyViewModel>> GetCompanies();
        public Task<CompanyViewModel> GetByID(int id);
        public Task<CompanyViewModel> GetCompanyByName(string name);
        public Task<CompanyViewModel> RegisterAsync(CompanyRequestDto requestDto);
        public Task<bool> Verify(int id);
    }
}
