using MediatR;

namespace ires.Application.Commands.RentalCharge
{
    public record DeleteRentalChargeCommand(long chargeid) : IRequest
    {
    }
}
