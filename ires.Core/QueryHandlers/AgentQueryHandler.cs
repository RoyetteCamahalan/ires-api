using AutoMapper;
using ires.Core.Queries.Agent;
using ires.Core.ViewModels;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.Employee;
using ires.Infrastructure.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.QueryHandlers
{
    public class AgentQueryHandler(IAgentService _agentService, IMapper _mapper) :
        IRequestHandler<GetAgentsQuery, PaginatedResult<AgentViewModel>>,
        IRequestHandler<GetAgentbyGuidQuery, AgentViewModel>
    {
        public async Task<PaginatedResult<AgentViewModel>> Handle(GetAgentsQuery request, CancellationToken cancellationToken)
        {
            var data = await _agentService.GeAgents(request.data);
            return _mapper.Map<PaginatedResult<AgentViewModel>>(data);
        }

        public async Task<AgentViewModel> Handle(GetAgentbyGuidQuery request, CancellationToken cancellationToken)
        {
            var data =  await _agentService.FindAgentByGuid(request.guid);
            return _mapper.Map<AgentViewModel>(data);
        }
    }
}
