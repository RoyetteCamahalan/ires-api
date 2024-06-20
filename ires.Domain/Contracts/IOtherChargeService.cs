using ires.Domain.DTO.OtherFee;

namespace ires.Domain.Contracts
{
    public interface IOtherChargeService
    {
        public Task<OtherFeeViewModel> CreateOtherFee(OtherFeeRequestDto requestDto);
        public Task<bool> UpdateOtherFee(OtherFeeRequestDto requestDto);
        public Task<ICollection<OtherFeeViewModel>> GetOtherFees(int companyID, string search, bool viewAll);
        public Task<OtherFeeViewModel> GetOtherFee(long id);
    }
}
