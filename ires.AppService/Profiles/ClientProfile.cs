using AutoMapper;
using ires.Domain.DTO.Client;
using Entities = ires.Infrastructure.Entities;

namespace ires.AppService.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<ClientRequestDto, Entities.Client>();
            CreateMap<Entities.Client, ClientViewModel>();
        }
    }
}
