using ires.Core.ViewModels;
using MediatR;

namespace ires.Core.Commands.LotModel
{
    public record UpdateLotModelCommand(
        long id,
        string name) : IRequest
    {
    }
}
