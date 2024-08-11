using AutoMapper;
using ires.Application.ViewModels;
using KeylessEntities = ires.Infrastructure.Keyless;

namespace ires.AppService.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<KeylessEntities.Payable, PayableViewModel>();
        }
    }
}
