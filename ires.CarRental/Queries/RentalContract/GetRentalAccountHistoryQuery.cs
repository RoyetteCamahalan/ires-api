using ires.Application.ViewModels;
using MediatR;

namespace ires.Application.Queries.RentalContract
{
    public record GetRentalAccountHistoryQuery(long id) : IRequest<IEnumerable<RentalHistoryViewModel>>
    {
    }
}
