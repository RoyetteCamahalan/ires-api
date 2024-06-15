using ires.Domain.DTO.OtherCharge;

namespace ires.Domain.Contracts
{
    public interface IChargeService
    {
        public Task<ICollection<OtherChargeViewModel>> GetOtherCharges(long surveyID, string search);
        public Task<ICollection<OtherChargeViewModel>> GetOtherCharges(long invoiceNo, long lotID, string search);
        public Task<OtherChargeViewModel> GetOtherChargeByID(long id);
        public Task<OtherChargeViewModel> Create(OtherChargeRequestDto requestDto);
        public Task<bool> Update(OtherChargeRequestDto requestDto);
    }
}
