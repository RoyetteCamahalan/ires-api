using ires.Application.ViewModels;
using MediatR;

namespace ires.Application.Queries.Booking
{
    public record GetBookingByGuidQuery(Guid guid) : IRequest<BookingViewModel>
    {
    }
}
