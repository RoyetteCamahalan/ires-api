using ires.Domain.DTO.Company;

namespace ires.Domain.Contracts
{
    public interface ICompanyService
    {
        public Task<ICollection<CompanyViewModel>> GetCompanies();
        public Task<CompanyViewModel> GetByID(int id);
        public Task<CompanyViewModel> GetCompanyByName(string name);
        public Task<CompanyViewModel> RegisterAsync(CompanyRequestDto requestDto);
        public Task<bool> Verify(int id);
        public Task<bool> CompleteTour(int id);
    }
}
