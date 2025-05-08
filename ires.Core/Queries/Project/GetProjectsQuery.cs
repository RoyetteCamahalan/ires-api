using ires.Core.ViewModels;
using ires.Domain.Common;
using MediatR;

namespace ires.Core.Queries.Agent
{
    public record GetProjectsQuery(PaginationRequest data) : IRequest<PaginatedResult<ProjectViewModel>>
    {
    }
}
