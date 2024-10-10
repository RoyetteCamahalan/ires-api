using ires.Application.ViewModels;
using ires.Domain.Enumerations;
using MediatR;

namespace ires.Application.Commands.Booking
{
    public record CreateBookingCommand(
        long clientid,
        long carid,
        DateTime startdate,
        DateTime enddate,
        int noofdays,
        BookingRateType ratetype,
        decimal rate,
        string drivername,
        bool isselfdrive,
        string remarks) : IRequest<BookingViewModel>
    {
    }
}
