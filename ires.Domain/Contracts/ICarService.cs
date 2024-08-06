using ires.Domain.Common;
using ires.Domain.Models;

namespace ires.Domain.Contracts
{
    public interface ICarService
    {
        Task<Car> Create(Car car);
        Task Update(Car car);
        Task<Car> GetCarById(long id);
        Task<bool> IsPlateNumberUnique(long id, string plateNumber);
        Task<PaginatedResult<Car>> GetAllCars(PaginationRequest request);
    }
}
