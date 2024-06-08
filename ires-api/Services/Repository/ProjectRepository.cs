using AutoMapper;
using ires_api.Data;
using ires_api.DTO.Project;
using ires_api.DTO.RentalUnit;
using ires_api.Enumerations;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
{
    public class ProjectRepository : IProjectService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ProjectRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<RentalProjectViewModel> Create(ProjectRequestDto projectRequest)
        {
            Project project = _mapper.Map<Project>(projectRequest);
            _dataContext.projects.Add(project);
            await _dataContext.SaveChangesAsync();
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

        public async Task<ICollection<RentalProjectViewModel>> GetRentalProperties(long companyID, string search)
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
            }
            return _mapper.Map<RentalProjectViewModel>(project);
        }

        public async Task<RentalUnitViewModel> CreateRentalUnit(RentalUnitRequestDto request)
        {
            var entity = _mapper.Map<RentalProperty>(request);
            if (request.isactive)
                entity.status = RentalPropertyStatus.Vacant;
            else
                entity.status = RentalPropertyStatus.Inactive;
            _dataContext.rentalProperties.Add(entity);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<RentalUnitViewModel>(entity);
        }

        public async Task<bool> UpdateRentalUnit(RentalUnitRequestDto request)
        {
            var entity = await GetRentalUnitById(request.propertyid);
            if (entity != null)
            {
                entity.propertyname = request.propertyname;
                entity.area = request.area;
                entity.monthlyrent = request.monthlyrent;
                if (entity.status != RentalPropertyStatus.Occupied)
                {
                    if (request.isactive)
                        entity.status = RentalPropertyStatus.Vacant;
                    else
                        entity.status = RentalPropertyStatus.Inactive;
                }
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private async Task<RentalProperty> GetRentalUnitById(long id)
        {
            return await _dataContext.rentalProperties.FindAsync(id);
        }

        public async Task<RentalUnitViewModel> GetRentalUnitByIdAsync(long id)
        {
            var entity = await GetRentalUnitById(id);
            return _mapper.Map<RentalUnitViewModel>(entity);
        }

        public async Task<ICollection<RentalUnitViewModel>> GetRentalUnits(long companyID, long projectID, string search)
        {
            var result = await _dataContext.rentalProperties.Include(x => x.project).Where(x => x.projectid == projectID
                && x.project.companyid == companyID && (x.propertyname.Contains(search) || x.area.Contains(search)))
                .OrderBy(x => x.propertyname).ToListAsync();
            var data = _mapper.Map<ICollection<RentalUnitViewModel>>(result);
            //foreach (var item in data)
            //{
            //    if(item.status == RentalPropertyStatus.Occupied)
            //    {
            //        var contract
            //    }
            //    var property = result.Where(x => x.propertyid == item.propertyid).First();
            //    item.noofunits = property.rentalProperties.Count();
            //    item.noofoccupiedunits = property.rentalProperties.Where(x => x.status == RentalPropertyStatus.Occupied).Count();
            //}
            return data;
        }

        public async Task<ICollection<RentalUnitViewModel>> GetAvailableRentalUnits(long companyID, string search)
        {
            var result = await _dataContext.rentalProperties.Include(x => x.project).Where(x => x.project.companyid == companyID
                && x.status == RentalPropertyStatus.Vacant && (x.propertyname.Contains(search) || x.area.Contains(search)))
                .OrderBy(x => x.project.propertyname).ThenBy(x => x.propertyname).ToListAsync();
            var data = _mapper.Map<ICollection<RentalUnitViewModel>>(result);
            return data;
        }
    }
}
