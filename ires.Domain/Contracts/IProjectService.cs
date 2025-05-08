using ires.Domain.Common;
using ires.Domain.Enumerations;
using ires.Domain.Models;

namespace ires.Domain.Contracts
{
    public interface IProjectService
    {
        public Task<Project> Create(Project request);

        public Task Update(Project request);
        public Task<Project> GetProjectByIdAsync(long id);
        public Task<Project> GetProjectByGuid(Guid guid);
        public Task<PaginatedResult<Project>> GetProjects(PaginationRequest request);
        public Task<ICollection<Project>> GetRentalProperties(string search);
        public Task<bool> IsNameUnique(string propertyName);



        public Task<RentalUnit> CreateRentalUnit(RentalUnit request);
        public Task UpdateRentalUnit(RentalUnit request);
        public Task UpdateRentalUnitStatus(long id, RentalPropertyStatus status);
        public Task<RentalUnit> GetRentalUnitByIdAsync(long id);
        public Task<ICollection<RentalUnit>> GetRentalUnits(long projectID, string search);
        public Task<ICollection<RentalUnit>> GetAvailableRentalUnits(string search);
    }
}
