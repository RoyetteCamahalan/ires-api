using ires.Core.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.Commands.Agent
{
    public record CreateAgentCommand(
        string firstname,
        string lastname,
        string contactno,
        string address,
        string email,
        string tinnumber,
        long? upline_id) : IRequest<AgentViewModel>
    {
    }
}
