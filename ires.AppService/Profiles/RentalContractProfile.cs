using AutoMapper;
using ires.Domain.DTO.RentalContract;
using Entities = ires.Infrastructure.Entities;

namespace ires.AppService.Profiles
{
    public class RentalContractProfile : Profile
    {
        public RentalContractProfile()
        {

            CreateMap<RentalContractRequestDto, Entities.RentalContract>()
                .ForMember(x => x.rentalContractDetails, opt => opt.Ignore())
                .ForMember(x => x.term, opt => opt.MapFrom(y => y.term ?? 0))
                .ForMember(x => x.noofmonthadvance, opt => opt.MapFrom(y => y.noofmonthadvance ?? 0))
                .ForMember(x => x.deposit, opt => opt.MapFrom(y => y.deposit ?? 0))
                .ForMember(x => x.monthlypenalty, opt => opt.MapFrom(y => y.monthlypenalty ?? 0))
                .ForMember(x => x.penaltyextension, opt => opt.MapFrom(y => y.penaltyextension ?? 0));
            CreateMap<Entities.RentalContract, RentalContractViewModel>();
        }
    }
}
