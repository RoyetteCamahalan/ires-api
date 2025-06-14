using ires.Core.ViewModels;
using ires.Domain.Common;
using MediatR;

namespace ires.Core.Queries.Lot
{
    public record GetLotsQuery(long projectId, PaginationRequest data) : IRequest<PaginatedResult<LotViewModel>>
    {
    }
}
