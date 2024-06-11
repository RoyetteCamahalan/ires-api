using ires_api.DTO.Project;
using ires_api.DTO.RentalUnit;
using ires_api.Enumerations;

namespace ires_api.Services.Interface
{
    public interface IProjectService
    {
        public Task<RentalProjectViewModel> Create(ProjectRequestDto projectRequest);

        public Task<RentalProjectViewModel> Update(ProjectRequestDto projectRequest);
        public Task<RentalProjectViewModel> GetProjectByIdAsync(long id);
        public Task<ICollection<RentalProjectViewModel>> GetRentalProperties(long companyID, string search);



        public Task<RentalUnitViewModel> CreateRentalUnit(RentalUnitRequestDto request);
        public Task<bool> UpdateRentalUnit(RentalUnitRequestDto request);
        public Task UpdateRentalUnitStatus(long id, RentalPropertyStatus status);
        public Task<RentalUnitViewModel> GetRentalUnitByIdAsync(long id);
        public Task<ICollection<RentalUnitViewModel>> GetRentalUnits(long companyID, long projectID, string search);
        public Task<ICollection<RentalUnitViewModel>> GetAvailableRentalUnits(long companyID, string search);
    }
}
