using ires.Domain.Enumerations;
using MediatR;

namespace ires.Application.Commands.Booking
{
    public record UpdateBookingCommand(
        long id,
        long clientid,
        long carid,
        DateTime startdate,
        DateTime enddate,
        int noofdays,
        BookingRateType ratetype,
        decimal rate,
        string drivername,
        bool isselfdrive,
        string remarks) : IRequest
    {
    }
}
