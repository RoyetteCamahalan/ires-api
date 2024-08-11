using AutoMapper;
using ires.Application.Commands.General;
using ires.Application.Commands.RentalContract;
using ires.AppService.Dto.RentalContract;
using ires.Domain.DTO.RentalContract;
using ires.Domain.Models;

namespace ires.AppService.Profiles
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            CreateMap<CreateRentalContractRequestDto, CreateRentalContractCommand>();
            CreateMap<CreateRentalContractCommand, RentalContract>();
            CreateMap<UpdateRentalContractRequestDto, UpdateRentalContractCommand>();
            CreateMap<UpdateRentalContractCommand, RentalContract>();
            CreateMap<SendRentalSOARequestDto, SendRentalSOACommand>();
            CreateMap<SendRentalSOACommand, MailingInfo>();
        }
    }
}
