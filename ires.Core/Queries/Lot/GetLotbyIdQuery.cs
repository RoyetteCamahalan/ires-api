using ires.Core.ViewModels;
using MediatR;

namespace ires.Core.Queries.Lot
{
    public record GetLotbyIdQuery(long id) : IRequest<LotViewModel>
    {
    }
}
