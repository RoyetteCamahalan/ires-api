using ires.Application.ViewModels;
using MediatR;

namespace ires.Application.Commands.RentalCharge
{
    public record CreateRentalChargeCommand(
        long chargeid,
        long contractid,
        long otherfeeid,
        DateTime chargedate,
        decimal chargeamount,
        decimal interestpercentage) : IRequest<RentalChargeViewModel>
    {
    }
}
