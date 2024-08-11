using ires.Application.ViewModels;
using MediatR;

namespace ires.Application.Queries.RentalCharge
{
    public record GetRentalChargeByIdQuery(long id) : IRequest<RentalChargeViewModel>
    {
    }
}
