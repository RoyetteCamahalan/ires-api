using ires.CarRental.ViewModels;
using MediatR;

namespace ires.CarRental.Queries.Car
{
    public record GetCarByIdQuery(long id) : IRequest<CarViewModel>
    {
    }
}
