using ires.Application.ViewModels;
using ires.Domain.Common;
using MediatR;

namespace ires.Application.Queries.Booking
{
    public record GetAllBookingsQuery(PaginationRequest data) : IRequest<PaginatedResult<BookingViewModel>>
    {
    }
}
