using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IChargeService
    {
        public ICollection<OtherCharge> GetOtherCharges(long surveyID, string search);
        public ICollection<OtherCharge> GetOtherCharges(long invoiceNo, long lotID, string search);
        public OtherCharge GetOtherChargeByID(long id);
        public OtherCharge Create(OtherCharge otherCharge);
        public OtherCharge Update(OtherChargeRequestDto requestDto);
    }
}
