using MediatR;

namespace ires.Application.Commands.RentalContract
{
    public record ReComputeRentalContractCommand(long id) : IRequest
    {
    }
}
