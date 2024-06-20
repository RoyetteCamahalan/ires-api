using AutoMapper;
using ires.Domain.Contracts;
using ires.Domain.DTO.Project;
using ires.Domain.DTO.RentalContract;
using ires.Domain.DTO.RentalUnit;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public ProjectRepository(DataContext dataContext, IMapper mapper, ILogService logService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logService = logService;
        }
        public async Task<RentalProjectViewModel> Create(ProjectRequestDto projectRequest)
        {
            Project project = _mapper.Map<Project>(projectRequest);
            _dataContext.projects.Add(project);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(project.companyid, projectRequest.createdbyid, AppModule.Projects, "Create Project", "Create New Record : " + project.propertyid + " - " + project.propertyname, 0);
            return _mapper.Map<RentalProjectViewModel>(project);
        }

        private async Task<Project> GetProjectById(long id)
        {
            return await _dataContext.projects.FindAsync(id);
        }

        public async Task<RentalProjectViewModel> GetProjectByIdAsync(long id)
        {
            var project = await GetProjectById(id);
            return _mapper.Map<RentalProjectViewModel>(project);
        }

        public async Task<ICollection<RentalProjectViewModel>> GetRentalProperties(int companyID, string search)
        {
            var result = await _dataContext.projects.Include(x => x.rentalProperties).Where(x => x.companyid == companyID && x.projectypeid == ProjectType.Rental
                && (x.propertyname.Contains(search) || x.address.Contains(search)))
                .OrderBy(x => x.propertyname).ToListAsync();
            var data = _mapper.Map<ICollection<RentalProjectViewModel>>(result);
            foreach (var item in data)
            {
                var property = result.Where(x => x.propertyid == item.propertyid).First();
                item.noofunits = property.rentalProperties.Count;
                item.noofoccupiedunits = property.rentalProperties.Where(x => x.status == RentalPropertyStatus.Occupied).Count();
            }
            return data;
        }

        public async Task<RentalProjectViewModel> Update(ProjectRequestDto projectRequest)
        {
            Project project = await GetProjectById(projectRequest.propertyid);
            if (project != null)
            {
                _dataContext.Entry(project).CurrentValues.SetValues(projectRequest);
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(project.companyid, projectRequest.updatedbyid, AppModule.Projects, "Update Project", "Update Record : " + project.propertyid + " - " + project.propertyname, 0);
            }
            return _mapper.Map<RentalProjectViewModel>(project);
        }

        public async Task<RentalUnitViewModel> CreateRentalUnit(RentalUnitRequestDto requestDto)
        {
            var entity = _mapper.Map<RentalProperty>(requestDto);
            if (requestDto.isactive)
                entity.status = RentalPropertyStatus.Vacant;
            else
                entity.status = RentalPropertyStatus.Inactive;
            _dataContext.rentalProperties.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(requestDto.companyid, requestDto.createdbyid, AppModule.Projects, "Create Rental Unit", "Create New Record : " + entity.propertyid.ToString() + "-" + entity.propertyname, 0);
            return _mapper.Map<RentalUnitViewModel>(entity);
        }

        public async Task<bool> UpdateRentalUnit(RentalUnitRequestDto requestDto)
        {
            var entity = await GetRentalUnitById(requestDto.propertyid);
            if (entity != null)
            {
                entity.propertyname = requestDto.propertyname;
                entity.area = requestDto.area;
                entity.monthlyrent = requestDto.monthlyrent;
                if (entity.status != RentalPropertyStatus.Occupied)
                {
                    if (requestDto.isactive)
                        entity.status = RentalPropertyStatus.Vacant;
                    else
                        entity.status = RentalPropertyStatus.Inactive;
                }
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(entity.project.companyid, requestDto.updatedbyid, AppModule.Projects, "Update Rental Unit", "Update Record : " + entity.propertyid.ToString() + "-" + entity.propertyname, 0);
                return true;
            }
            return false;
        }
        private async Task<RentalProperty> GetRentalUnitById(long id)
        {
            return await _dataContext.rentalProperties.Include(x => x.project).FirstOrDefaultAsync(x => x.propertyid == id);
        }
        public async Task<RentalUnitViewModel> GetRentalUnitByIdAsync(long id)
        {
            var entity = await GetRentalUnitById(id);
            return _mapper.Map<RentalUnitViewModel>(entity);
        }

        public async Task<ICollection<RentalUnitViewModel>> GetRentalUnits(int companyID, long projectID, string search)
        {
            var result = await _dataContext.rentalProperties.Include(x => x.project).Where(x => x.projectid == projectID
                && x.project.companyid == companyID && (x.propertyname.Contains(search) || x.area.Contains(search)))
                .OrderBy(x => x.propertyname).ToListAsync();
            var data = _mapper.Map<ICollection<RentalUnitViewModel>>(result);
            foreach (var item in data)
            {
                if (item.status == RentalPropertyStatus.Occupied)
                {
                    var contract = await GetContractByUnit(item.propertyid);
                    item.tenant = contract.client.fullname;
                }
            }
            return data;
        }

        public async Task<RentalContractViewModel> GetContractByUnit(long propertyID)
        {
            var result = await _dataContext.rentalContractDetails.Include(x => x.rentalContract).ThenInclude(x => x.client)
                .Where(x => x.rentalContract.status == RentStatus.Active && x.propertyid == propertyID)
                .Select(x => x.rentalContract).FirstOrDefaultAsync();
            return _mapper.Map<RentalContractViewModel>(result);
        }

        public async Task<ICollection<RentalUnitViewModel>> GetAvailableRentalUnits(int companyID, string search)
        {
            var result = await _dataContext.rentalProperties.Include(x => x.project).Where(x => x.project.companyid == companyID
                && x.status == RentalPropertyStatus.Vacant && (x.propertyname.Contains(search) || x.area.Contains(search)))
                .OrderBy(x => x.project.propertyname).ThenBy(x => x.propertyname).ToListAsync();
            var data = _mapper.Map<ICollection<RentalUnitViewModel>>(result);
            return data;
        }

        public async Task UpdateRentalUnitStatus(long id, RentalPropertyStatus status)
        {
            var entity = await GetRentalUnitById(id);
            if (entity != null)
            {
                entity.status = status;
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
