using AutoMapper;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Domain.Models;
using ires.Infrastructure.Extensions;
using ires.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class CarRepository(DataContext _dataContext,
        IMapper _mapper,
        ICurrentUserService _currentUserService,
        ILogService _logService) : ICarService
    {
        private async Task<Entities.Car> GetCarByID(long id)
        {
            return await _dataContext.cars.Include(x => x.carType).FirstOrDefaultAsync(x => x.id == id);
        }
        public async Task<Car> Create(Car car)
        {
            var entity = _mapper.Map<Entities.Car>(car);
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.cars.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Car, "Create Car", "New Car: " + entity.id + " - " + entity.platenumber);
            return _mapper.Map<Car>(entity);
        }

        public async Task<PaginatedResult<Car>> GetAllCars(PaginationRequest request)
        {
            var query = _dataContext.cars.Include(x => x.carType)
                .Where(x => (x.name.Contains(request.searchString) || x.platenumber.Contains(request.searchString))).AsQueryable();
            return await query.AsPaginatedResult<Entities.Car, Car>(request, _mapper.ConfigurationProvider);
        }

        public async Task<Car> GetCarById(long id)
        {
            var entity = await GetCarByID(id) ?? throw new EntityNotFoundException();
            return _mapper.Map<Car>(entity);

        }

        public async Task<bool> IsPlateNumberUnique(long id, string plateNumber)
        {
            return !await _dataContext.cars.Where(x => x.id != id && x.platenumber == plateNumber).AnyAsync();
        }

        public async Task Update(Car car)
        {
            var entity = await GetCarByID(car.id) ?? throw new EntityNotFoundException();
            entity.name = car.name;
            entity.model = car.model;
            entity.platenumber = car.platenumber;
            entity.year = car.year;
            entity.typeid = car.typeid;
            entity.status = car.status;
            entity.updatedbyid = _currentUserService.employeeid;
            entity.dateupdated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Car, "Updated Car", "Updated Car: " + entity.id + " - " + entity.platenumber);
        }
    }
}
