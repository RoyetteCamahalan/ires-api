using ires.Application.ViewModels;
using MediatR;

namespace ires.Application.Queries.Car
{
    public record GetCarByIdQuery(long id) : IRequest<CarViewModel>
    {
    }
}
