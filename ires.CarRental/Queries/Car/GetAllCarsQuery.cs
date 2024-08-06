using ires.CarRental.ViewModels;
using ires.Domain.Common;
using MediatR;

namespace ires.CarRental.Queries.Car
{
    public record GetAllCarsQuery(PaginationRequest data) : IRequest<PaginatedResult<CarViewModel>>
    {
    }
}
