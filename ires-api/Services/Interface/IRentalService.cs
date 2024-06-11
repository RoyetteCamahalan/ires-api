using ires_api.DTO.RentalContract;
using ires_api.DTO.RentalUnit;

namespace ires_api.Services.Interface
{
    public interface IRentalService
    {
        public Task<RentalContractViewModel> Create(RentalContractRequestDto requestDto);
        public Task<bool> Update(RentalContractRequestDto requestDto);
        public Task<RentalContractViewModel> Get(long companyID, long contractID);
        public Task<ICollection<RentalContractViewModel>> GetAll(long companyID, string search, int filterByID);
        public Task<ICollection<RentalUnitViewModel>> GetProperties(long contractID);
        public Task<RentalContractViewModel> GetContractByUnit(long propertyID);
    }
}
