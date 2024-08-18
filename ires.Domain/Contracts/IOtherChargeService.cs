using ires.Domain.Common;
using ires.Domain.DTO.OtherFee;

namespace ires.Domain.Contracts
{
    public interface IOtherChargeService
    {
        public Task<OtherFeeViewModel> CreateOtherFee(OtherFeeRequestDto requestDto);
        public Task UpdateOtherFee(OtherFeeRequestDto requestDto);
        public Task<PaginatedResult<OtherFeeViewModel>> GetOtherFees(PaginationRequest request);
        public Task<OtherFeeViewModel> GetOtherFee(long id);
    }
}
