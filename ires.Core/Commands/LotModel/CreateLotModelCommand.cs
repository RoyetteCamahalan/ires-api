using ires.Core.ViewModels;
using MediatR;

namespace ires.Core.Commands.LotModel
{
    public record CreateLotModelCommand(
        long project_id,
        string name) : IRequest<LotModelViewModel>
    {
    }
}
