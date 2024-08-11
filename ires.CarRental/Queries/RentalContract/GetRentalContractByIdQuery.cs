using ires.Application.ViewModels;
using MediatR;

namespace ires.Application.Queries.RentalContract
{
    public record GetRentalContractByIdQuery(long id) : IRequest<RentalContractViewModel>
    {
    }
}
