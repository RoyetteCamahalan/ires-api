using ires.Domain.Common;
using ires.Domain.DTO;
using ires.Domain.DTO.Payment;
using ires.Domain.DTO.RentalCharge;
using ires.Domain.DTO.RentalContract;
using ires.Domain.DTO.RentalContractDetail;
using ires.Domain.DTO.RentalUnit;

namespace ires.Domain.Contracts
{
    public interface IRentalService
    {
        public Task<RentalContractViewModel> Create(RentalContractRequestDto requestDto);
        public Task Update(RentalContractRequestDto requestDto);
        public Task<RentalContractViewModel> Get(long contractID);
        public Task<ICollection<RentalContractDetailViewModel>> GetDetails(long contractID);
        public Task<PaginatedResult<RentalContractViewModel>> GetAll(PaginationRequest request);
        public Task<ICollection<RentalUnitViewModel>> GetProperties(long contractID);
        public Task<string> GetPropertiesAsString(long contractID);
        public Task RecomputeContract(long contractID);
        public Task<ICollection<RentalHistoryViewModel>> GetAccountHistory(long contractID);
        public Task<ICollection<PayableViewModel>> GetSOA(long contractID);


        public Task<RentalChargeViewModel> GetRentalCharge(long id);
        public Task<bool> RentalChageHasPayment(long id);
        public Task<RentalChargeViewModel> CreateOtherCharge(RentalChargeRequestDto requestDto);
        public Task UpdateOtherCharge(RentalChargeRequestDto requestDto);
        public Task DeleteOtherCharge(long id);
        public Task UpdateContractStatus(RentalTerminateRequestDto requestDto);
        public Task<int> CountActiveUnits();
        public Task<int> CountAvailableUnits();
        public Task<int> CountActiveContracts();

        public Task<FileDataViewModel> GenerateSOA(long contractid);
        public Task<bool> SendSOA(SendMailRequestDto requestDto);
    }
}
