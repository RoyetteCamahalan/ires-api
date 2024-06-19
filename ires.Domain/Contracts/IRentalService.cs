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
        public Task<bool> Update(RentalContractRequestDto requestDto);
        public Task<RentalContractViewModel> Get(long companyID, long contractID);
        public Task<ICollection<RentalContractDetailViewModel>> GetDetails(long contractID);
        public Task<ICollection<RentalContractViewModel>> GetAll(long companyID, string search, int filterByID);
        public Task<ICollection<RentalUnitViewModel>> GetProperties(long contractID);
        public Task<string> GetPropertiesAsString(long contractID);
        public Task RecomputeContract(long contractID);
        public Task<ICollection<RentalHistoryViewModel>> GetAccountHistory(long companyID, long contractID);
        public Task<ICollection<PayableViewModel>> GetSOA(long companyID, long contractID);


        public Task<RentalChargeViewModel> GetRentalCharge(long id);
        public Task<bool> RentalChageHasPayment(long id);
        public Task<RentalChargeViewModel> CreateOtherCharge(RentalChargeRequestDto requestDto);
        public Task<bool> UpdateOtherCharge(RentalChargeRequestDto requestDto);
        public Task<bool> DeleteOtherCharge(long id);
        public Task<bool> UpdateContractStatus(RentalTerminateRequestDto requestDto);
    }
}
