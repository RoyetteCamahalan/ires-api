using ires.Domain.Common;
using ires.Domain.Models;

namespace ires.Domain.Contracts
{
    public interface IBookingService
    {
        public Task<Booking> Create(Booking booking);
        public Task Update(Booking booking);
        public Task<Booking> GetBookingByGuId(Guid guid);
        public Task<PaginatedResult<Booking>> GetAllBookings(PaginationRequest request);
    }
}
