using AutoMapper;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.Project;
using ires.Domain.DTO.RentalContract;
using ires.Domain.DTO.RentalUnit;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using ires.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class ProjectRepository(
        DataContext _dataContext,
        IMapper _mapper,
        ILogService _logService,
        ICurrentUserService _currentUserService) : IProjectService
    {

        public async Task<RentalProjectViewModel> Create(ProjectRequestDto projectRequest)
        {
            Project project = _mapper.Map<Project>(projectRequest);
            project.companyid = _currentUserService.companyid;
            _dataContext.projects.Add(project);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Projects, "Create Project", "Create New Record : " + project.propertyid + " - " + project.propertyname);
            return _mapper.Map<RentalProjectViewModel>(project);
        }

        private async Task<Project> GetProjectById(long id)
        {
            return await _dataContext.projects.FindAsync(id) ?? throw new EntityNotFoundException();
        }

        public async Task<RentalProjectViewModel> GetProjectByIdAsync(long id)
        {
            var project = await GetProjectById(id);
            return _mapper.Map<RentalProjectViewModel>(project);
        }

        public async Task<PaginatedResult<RentalProjectViewModel>> GetRentalProperties(PaginationRequest request)
        {
            var query = _dataContext.projects.Include(x => x.rentalProperties)
                .Where(x => x.companyid == _currentUserService.companyid && x.projectypeid == ProjectType.Rental
                && (x.propertyname.Contains(request.searchString) || x.address.Contains(request.searchString)))
                .OrderBy(x => x.propertyname).AsQueryable();
            var data = await query.AsPaginatedResult<Project, RentalProjectViewModel>(request, _mapper.ConfigurationProvider);
            foreach (var item in data.data)
            {
                var properties = await _dataContext.rentalProperties.Where(x => x.projectid == item.propertyid).ToListAsync();
                item.noofunits = properties.Count;
                item.noofoccupiedunits = properties.Where(x => x.status == RentalPropertyStatus.Occupied).Count();
            }
            return data;
        }

        public async Task<RentalProjectViewModel> Update(ProjectRequestDto projectRequest)
        {
            Project project = await GetProjectById(projectRequest.propertyid);
            _dataContext.Entry(project).CurrentValues.SetValues(projectRequest);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Projects, "Update Project", "Update Record : " + project.propertyid + " - " + project.propertyname);
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
            await _logService.SaveLogAsync(AppModule.Projects, "Create Rental Unit", "Create New Record : " + entity.propertyid.ToString() + "-" + entity.propertyname);
            return _mapper.Map<RentalUnitViewModel>(entity);
        }

        public async Task UpdateRentalUnit(RentalUnitRequestDto requestDto)
        {
            var entity = await GetRentalUnitById(requestDto.propertyid);
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
            await _logService.SaveLogAsync(AppModule.Projects, "Update Rental Unit", "Update Record : " + entity.propertyid.ToString() + "-" + entity.propertyname);
        }
        private async Task<RentalProperty> GetRentalUnitById(long id)
        {
            return await _dataContext.rentalProperties.Include(x => x.project)
                .FirstOrDefaultAsync(x => x.propertyid == id) ?? throw new EntityNotFoundException();
        }
        public async Task<RentalUnitViewModel> GetRentalUnitByIdAsync(long id)
        {
            var entity = await GetRentalUnitById(id);
            return _mapper.Map<RentalUnitViewModel>(entity);
        }

        public async Task<PaginatedResult<RentalUnitViewModel>> GetRentalUnits(PaginationRequest request)
        {
            var query = _dataContext.rentalProperties.Include(x => x.project)
                .Where(x => x.projectid == request.projectID
                && x.project.companyid == _currentUserService.companyid &&
                (x.propertyname.Contains(request.searchString) || x.area.Contains(request.searchString)))
                .OrderBy(x => x.propertyname).AsQueryable();
            var data = await query.AsPaginatedResult<RentalProperty, RentalUnitViewModel>(request, _mapper.ConfigurationProvider);
            foreach (var item in data.data)
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
                .Select(x => x.rentalContract).FirstOrDefaultAsync() ?? throw new EntityNotFoundException();
            return _mapper.Map<RentalContractViewModel>(result);
        }

        public async Task<PaginatedResult<RentalUnitViewModel>> GetAvailableRentalUnits(PaginationRequest request)
        {
            var query = _dataContext.rentalProperties.Include(x => x.project)
                .Where(x => x.project.companyid == _currentUserService.companyid
                && x.status == RentalPropertyStatus.Vacant && (x.propertyname.Contains(request.searchString) || x.area.Contains(request.searchString)))
                .OrderBy(x => x.project.propertyname).ThenBy(x => x.propertyname).AsQueryable();
            return await query.AsPaginatedResult<RentalProperty, RentalUnitViewModel>(request, _mapper.ConfigurationProvider);
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
