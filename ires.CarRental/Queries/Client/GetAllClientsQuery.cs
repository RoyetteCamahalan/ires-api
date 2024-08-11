using ires.Application.ViewModels;
using ires.Domain.Common;
using MediatR;

namespace ires.Application.Queries.Client
{
    public record GetAllClientsQuery(PaginationRequest data) : IRequest<PaginatedResult<ClientViewModel>>
    {
    }
}
