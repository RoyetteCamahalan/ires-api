using AutoMapper;
using ires.AppService.DTO.Agent;
using ires.Core.Commands.Agent;
using ires.Core.ViewModels;
using ires.Domain.Models;

namespace ires.AppService.Profiles
{
    public class AgentProfile : Profile
    {
        public AgentProfile()
        {
            CreateMap<CreateAgentRequestDto, CreateAgentCommand>();
            CreateMap<CreateAgentCommand, Agent>();
            CreateMap<UpdateAgentRequestDto, UpdateAgentCommand>();
            CreateMap<UpdateAgentCommand, Agent>();
            CreateMap<Infrastructure.Entities.Agent, Agent>().ReverseMap();
            CreateMap<Agent, AgentViewModel>();
        }
    }
}
