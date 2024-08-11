using MediatR;

namespace ires.Application.Commands.RentalCharge
{
    public record UpdateRentalChargeCommand(
        long contractid,
        long otherfeeid,
        DateTime chargedate,
        decimal chargeamount,
        decimal interestpercentage) : IRequest
    {
    }
}
