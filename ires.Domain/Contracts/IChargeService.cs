using ires.Domain.Common;
using ires.Domain.DTO.OtherCharge;

namespace ires.Domain.Contracts
{
    public interface IChargeService
    {
        public Task<PaginatedResult<OtherChargeViewModel>> GetOtherCharges(long surveyID, PaginationRequest request);
        public Task<ICollection<OtherChargeViewModel>> GetOtherCharges(long invoiceNo, long lotID, string search);
        public Task<OtherChargeViewModel> GetOtherChargeByID(long id);
        public Task<OtherChargeViewModel> Create(OtherChargeRequestDto requestDto);
        public Task Update(OtherChargeRequestDto requestDto);
    }
}
