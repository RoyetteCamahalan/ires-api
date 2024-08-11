using ires.Domain.Common;
using ires.Domain.Models;

namespace ires.Domain.Contracts
{
    public interface IRentalService
    {
        public Task<RentalContract> Create(RentalContract request);
        public Task<bool> Update(RentalContract request);
        public Task<RentalContract> Get(long contractID);
        public Task<ICollection<RentalContractDetail>> GetDetails(long contractID);
        public Task<PaginatedResult<RentalContract>> GetAll(PaginationRequest request);
        public Task<ICollection<RentalProperty>> GetProperties(long contractID);
        public Task<string> GetPropertiesAsString(long contractID);
        public Task RecomputeContract(long contractID);
        public Task<ICollection<RentalAccountHistory>> GetAccountHistory(long contractID);
        public Task<ICollection<Payable>> GetSOA(long contractID);


        public Task<RentalCharge> GetRentalCharge(long id);
        public Task<bool> RentalChageHasPayment(long id);
        public Task<RentalCharge> CreateOtherCharge(RentalCharge request);
        public Task<bool> UpdateOtherCharge(RentalCharge request);
        public Task<bool> DeleteOtherCharge(long id);
        public Task<bool> UpdateContractStatus(RentalContract request);
        public Task<int> CountActiveUnits();
        public Task<int> CountAvailableUnits();
        public Task<int> CountActiveContracts();

        public Task<FileData> GenerateSOA(long contractid);
        public Task<bool> SendSOA(MailingInfo info);
    }
}
