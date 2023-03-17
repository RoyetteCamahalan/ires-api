using ires_api.DTO;

namespace ires_api.Services.Interface
{
    public interface IPaymentService
    {
        public ICollection<PayableDto> GetPayables(long clientID, string search);
    }
}
