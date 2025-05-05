using ires.Core.ViewModels;
using ires.Domain.Common;
using MediatR;

namespace ires.Core.Queries.Project
{
    public record GetProjectbyIdQuery(long id) : IRequest<ProjectViewModel>
    {
    }
}
