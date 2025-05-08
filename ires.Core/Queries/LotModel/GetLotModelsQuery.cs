using ires.Core.ViewModels;
using ires.Domain.Common;
using MediatR;

namespace ires.Core.Queries.LotModel
{
    public record GetLotModelsQuery(long projectId, PaginationRequest data) : IRequest<PaginatedResult<LotModelViewModel>>
    {
    }
}
