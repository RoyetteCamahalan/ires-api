using ires.Core.ViewModels;
using ires.Domain.Common;
using MediatR;

namespace ires.Core.Queries.Lot
{
    public record GetAvailableLotsQuery(Guid project_guid, PaginationRequest data) : IRequest<PaginatedResult<LotViewModel>>
    {
    }
}
