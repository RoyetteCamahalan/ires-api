using ires.Domain.Enumerations;
using MediatR;

namespace ires.Application.Commands.RentalContract
{
    public record TerminateRentalContractCommand(
        long contractid,
        RentStatus status,
        DateTime? dateterminated) : IRequest
    {
    }
}
