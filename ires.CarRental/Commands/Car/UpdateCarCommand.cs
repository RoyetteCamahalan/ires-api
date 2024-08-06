using ires.Domain.Enumerations;
using MediatR;

namespace ires.CarRental.Commands.Car
{
    public record UpdateCarCommand(
        long id,
        string name,
        int typeid,
        string platenumber,
        string model,
        string year,
        CarStatus status) : IRequest
    {
    }
}
