using ires.Domain.Common;
using ires.Domain.DTO.Project;
using ires.Domain.DTO.RentalUnit;
using ires.Domain.Enumerations;

namespace ires.Domain.Contracts
{
    public interface IProjectService
    {
        public Task<RentalProjectViewModel> Create(ProjectRequestDto projectRequest);

        public Task<RentalProjectViewModel> Update(ProjectRequestDto projectRequest);
        public Task<RentalProjectViewModel> GetProjectByIdAsync(long id);
        public Task<PaginatedResult<RentalProjectViewModel>> GetRentalProperties(PaginationRequest request);



        public Task<RentalUnitViewModel> CreateRentalUnit(RentalUnitRequestDto request);
        public Task UpdateRentalUnit(RentalUnitRequestDto request);
        public Task UpdateRentalUnitStatus(long id, RentalPropertyStatus status);
        public Task<RentalUnitViewModel> GetRentalUnitByIdAsync(long id);
        public Task<PaginatedResult<RentalUnitViewModel>> GetRentalUnits(PaginationRequest request);
        public Task<PaginatedResult<RentalUnitViewModel>> GetAvailableRentalUnits(PaginationRequest request);
    }
}
