using AutoMapper;
using ires.Application.Queries.Booking;
using ires.Application.ViewModels;
using ires.Domain.Common;
using ires.Domain.Contracts;
using MediatR;

namespace ires.Application.QueryHandlers
{
    public class BookingQueryHandler(IBookingService _bookingService, IMapper _mapper) :
        IRequestHandler<GetBookingByGuidQuery, BookingViewModel>,
        IRequestHandler<GetAllBookingsQuery, PaginatedResult<BookingViewModel>>
    {
        public async Task<PaginatedResult<BookingViewModel>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<PaginatedResult<BookingViewModel>>(await _bookingService.GetAllBookings(request.data));
        }

        public async Task<BookingViewModel> Handle(GetBookingByGuidQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<BookingViewModel>(await _bookingService.GetBookingByGuId(request.guid));
        }
    }
}
