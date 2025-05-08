using ires.Core.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.Commands.Project
{
    public record CreateProjectCommand(
        string propertyname,
        string address,
        string alias,
        decimal area) : IRequest<ProjectViewModel>
    {
    }
}
