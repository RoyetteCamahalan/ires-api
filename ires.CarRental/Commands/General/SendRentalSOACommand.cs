using MediatR;

namespace ires.Application.Commands.General
{
    public record SendRentalSOACommand(
        long id,
        string email) : IRequest
    {
    }
}
