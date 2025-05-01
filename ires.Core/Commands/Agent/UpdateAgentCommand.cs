using ires.Core.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.Commands.Agent
{
    public record UpdateAgentCommand(
        Guid guid,
        string firstname,
        string lastname,
        string contactno,
        string address,
        string email,
        string tinnumber,
        bool isactive,
        long upline_id) : IRequest
    {
    }
}
