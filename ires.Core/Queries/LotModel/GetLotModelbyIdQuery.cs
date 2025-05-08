using ires.Core.ViewModels;
using MediatR;

namespace ires.Core.Queries.LotModel
{
    public record GetLotModelbyIdQuery(long id) : IRequest<LotModelViewModel>
    {
    }
}
