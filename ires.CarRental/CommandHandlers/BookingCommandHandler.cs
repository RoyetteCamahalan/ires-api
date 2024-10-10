using AutoMapper;
using ires.Application.Commands.Booking;
using ires.Application.ViewModels;
using ires.Domain.Contracts;
using ires.Domain.Models;
using MediatR;

namespace ires.Application.CommandHandlers
{
    public class BookingCommandHandler(IBookingService _bookingService, IMapper _mapper) :
        IRequestHandler<CreateBookingCommand, BookingViewModel>,
        IRequestHandler<UpdateBookingCommand>
    {
        public async Task<BookingViewModel> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var data = await _bookingService.Create(_mapper.Map<Booking>(request));
            return _mapper.Map<BookingViewModel>(data);
        }

        public async Task<Unit> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            await _bookingService.Create(_mapper.Map<Booking>(request));
            return new Unit();
        }
    }
}
