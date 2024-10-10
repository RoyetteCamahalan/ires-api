using AutoMapper;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Domain.Models;
using ires.Infrastructure.Data;
using ires.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class BookingRepository(
        DataContext _dataContext,
        ICurrentUserService _currentUserService,
        IMapper _mapper,
        ILogService _logService) : IBookingService
    {
        public async Task<Booking> Create(Booking booking)
        {
            var entity = _mapper.Map<Entities.Booking>(booking);
            entity.guid = Guid.NewGuid();
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.bookings.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Booking, "New Booking", entity);
            return _mapper.Map<Booking>(entity);
        }

        public async Task<PaginatedResult<Booking>> GetAllBookings(PaginationRequest request)
        {
            var query = _dataContext.bookings.Include(x => x.client).Include(x => x.car)
                .Where(x => ((int)x.status == request.filterBy || request.filterBy == 0)
                    && (x.client.fname + " " + x.client.lname).Contains(request.search) || x.car.name.Contains(request.searchString))
                    .OrderBy(x => x.startdate).AsQueryable();
            return await query.AsPaginatedResult<Entities.Booking, Booking>(request, _mapper.ConfigurationProvider);
        }
        private async Task<Entities.Booking> GetByID(long id)
        {
            return await _dataContext.bookings.Include(x => x.client).Include(x => x.car)
                .FirstOrDefaultAsync(x => x.id == id) ?? throw new EntityNotFoundException();
        }
        private async Task<Entities.Booking> GetByGuID(Guid guid)
        {
            return await _dataContext.bookings.Include(x => x.client).Include(x => x.car)
                .FirstOrDefaultAsync(x => x.guid == guid) ?? throw new EntityNotFoundException();
        }
        public async Task<Booking> GetBookingByGuId(Guid guid)
        {
            return _mapper.Map<Booking>(await GetByGuID(guid));
        }

        public async Task Update(Booking booking)
        {
            var entity = await GetByID(booking.id);
            entity.clientid = booking.clientid;
            entity.carid = booking.carid;
            entity.startdate = booking.startdate;
            entity.enddate = booking.enddate;
            entity.noofdays = booking.noofdays;
            entity.rate = booking.rate;
            entity.ratetype = booking.ratetype;
            entity.isselfdrive = booking.isselfdrive;
            entity.remarks = booking.remarks;
            entity.status = booking.status;
            entity.updatedbyid = _currentUserService.employeeid;
            entity.dateupdated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Booking, "Updated Booking", entity);
        }
    }
}
