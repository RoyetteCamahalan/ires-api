using ires.Application.ViewModels;
using ires.Domain.Common;
using MediatR;

namespace ires.Application.Queries.Car
{
    public record GetAllCarsQuery(PaginationRequest data) : IRequest<PaginatedResult<CarViewModel>>
    {
    }
}
