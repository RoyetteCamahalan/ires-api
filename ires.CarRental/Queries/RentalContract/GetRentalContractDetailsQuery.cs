using ires.Application.ViewModels;
using MediatR;

namespace ires.Application.Queries.RentalContract
{
    public record GetRentalContractDetailsQuery(long id) : IRequest<IEnumerable<RentalContractDetailViewModel>
    {
    }
}
