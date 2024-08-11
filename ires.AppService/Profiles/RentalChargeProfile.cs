using AutoMapper;
using ires.Application.Commands.RentalCharge;
using ires.Application.ViewModels;
using ires.AppService.Dto.RentalCharge;
using ires.Domain.Models;
using Entities = ires.Infrastructure.Entities;

namespace ires.AppService.Profiles
{
    public class RentalChargeProfile : Profile
    {
        public RentalChargeProfile()
        {
            CreateMap<CreateRentalChargeRequestDto, CreateRentalChargeCommand>();
            CreateMap<CreateRentalChargeCommand, RentalCharge>();
            CreateMap<UpdateRentalChargeRequestDto, UpdateRentalChargeCommand>();
            CreateMap<UpdateRentalChargeCommand, RentalCharge>();
            CreateMap<RentalCharge, RentalChargeViewModel>();
            CreateMap<RentalCharge, Entities.RentalCharge>().ReverseMap();
        }
    }
}
