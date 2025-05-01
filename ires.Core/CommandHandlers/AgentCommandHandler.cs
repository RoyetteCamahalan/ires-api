
using AutoMapper;
using ires.Core.Commands.Agent;
using ires.Core.ViewModels;
using ires.Domain.Contracts;
using ires.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.CommandHandlers
{
    public class AgentCommandHandler(IAgentService _agentService, IMapper _mapper) :
        IRequestHandler<CreateAgentCommand, AgentViewModel>,
        IRequestHandler<UpdateAgentCommand>
    {
        public async Task<AgentViewModel> Handle(CreateAgentCommand request, CancellationToken cancellationToken)
        {
            var data = await _agentService.Create(_mapper.Map<Agent>(request));
            return _mapper.Map<AgentViewModel>(data);
        }

        public async Task<Unit> Handle(UpdateAgentCommand request, CancellationToken cancellationToken)
        {
            await _agentService.Update(_mapper.Map<Agent>(request));
            return new Unit();
        }
    }
}
