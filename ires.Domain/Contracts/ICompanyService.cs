using ires.Domain.DTO.Company;

namespace ires.Domain.Contracts
{
    public interface ICompanyService
    {
        public Task<ICollection<CompanyViewModel>> GetCompanies();
        public Task<CompanyViewModel> GetByID();
        public Task<CompanyViewModel> GetCompanyByName(string name);
        public Task<CompanyViewModel> RegisterAsync(RegisterCompanyRequestDto requestDto);
        public Task Update(UpdateCompanyRequestDto requestDto);
        public Task Verify(int id);
        public Task CompleteTour(int id);
    }
}
