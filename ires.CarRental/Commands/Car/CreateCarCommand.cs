using ires.Application.ViewModels;
using ires.Domain.Enumerations;
using MediatR;

namespace ires.Application.Commands.Car
{
    public record CreateCarCommand(
        string name,
        int typeid,
        string platenumber,
        string model,
        string year,
        CarStatus status) : IRequest<CarViewModel>
    {
    }
}
