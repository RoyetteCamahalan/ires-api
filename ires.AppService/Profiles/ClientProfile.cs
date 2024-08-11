using AutoMapper;
using ires.Application.Commands.Client;
using ires.Application.ViewModels;
using ires.AppService.Dto.Client;
using ires.Domain.Models;
using Entities = ires.Infrastructure.Entities;

namespace ires.AppService.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<CreateClientRequestDto, CreateClientCommand>();
            CreateMap<CreateClientCommand, Client>();
            CreateMap<UpdateClientRequestDto, UpdateClientCommand>();
            CreateMap<UpdateClientCommand, Client>();
            CreateMap<Client, ClientViewModel>();
            CreateMap<Client, Entities.Client>().ReverseMap();
        }
    }
}
