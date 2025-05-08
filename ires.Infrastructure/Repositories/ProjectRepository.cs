using AutoMapper;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Domain.Models;
using ires.Infrastructure.Common;
using ires.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class ProjectRepository(DataContext dataContext, IMapper mapper, ILogService logService, ICurrentUserService _currentUserService) : IProjectService
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IMapper _mapper = mapper;
        private readonly ILogService _logService = logService;

        public async Task<Project> Create(Project request)
        {
            var project = _mapper.Map<Entities.Project>(request);
            project.guid = Guid.NewGuid();
            project.companyid = _currentUserService.companyid;
            _dataContext.projects.Add(project);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(project.companyid, _currentUserService.employeeid, AppModule.Projects, "Create Project", "Create New Record : " + project.propertyid + " - " + project.propertyname, 0);
            return _mapper.Map<Project>(project);
        }

        private async Task<Entities.Project> GetProjectById(long id)
        {
            return await _dataContext.projects.FindAsync(id) ?? throw new ObjectNotFoundException();
        }

        public async Task<Project> GetProjectByIdAsync(long id)
        {
            return _mapper.Map<Project>(await GetProjectById(id));
        }

        public async Task<Project> GetProjectByGuid(Guid guid)
        {
            var entity = await _dataContext.projects.FirstOrDefaultAsync(x => x.guid == guid);
            return _mapper.Map<Project>(entity);
        }
        
        public async Task<ICollection<Project>> GetRentalProperties(string search)
        {
            var result = await _dataContext.projects.Include(x => x.rentalProperties).Where(x => x.companyid == _currentUserService.companyid && x.projectypeid == ProjectType.Rental
                && (x.propertyname.Contains(search) || x.address.Contains(search)))
                .OrderBy(x => x.propertyname).ToListAsync();
            return _mapper.Map<ICollection<Project>>(result);
        }

        public async Task Update(Project request)
        {
            var project = await GetProjectById(request.propertyid);
            if (project != null)
            {
                project.propertyname = request.propertyname;
                project.address = request.address;
                if(project.projectypeid == ProjectType.Subdivision)
                {
                    project.alias = request.alias;
                    project.area = request.area;
                    //project.computationtype = request.computationtype;
                    //project.defaultcommission = request.defaultcommission;
                    project.com_percentage = request.com_percentage;
                    //project.compercentageoverterm = request.compercentageoverterm;
                    project.paymentterm = request.paymentterm;
                    project.interest = request.interest;
                    project.commissionterm = request.commissionterm;
                    project.paymentextension = request.paymentextension;
                    //project.allow_straight_monthly = request.allow_straight_monthly;
                    //project.withholding = request.withholding;
                    //project.interesttype = request.interesttype;
                    //project.addoninterestpermonth = request.addoninterestpermonth;
                }
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(project.companyid, _currentUserService.employeeid, AppModule.Projects, "Update Project", "Update Record : " + project.propertyid + " - " + project.propertyname, 0);
            }
        }

        public async Task<RentalUnit> CreateRentalUnit(RentalUnit requestDto)
        {
            var entity = _mapper.Map<Entities.RentalProperty>(requestDto);
            _dataContext.rentalProperties.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(_currentUserService.companyid, _currentUserService.employeeid, AppModule.Projects, "Create Rental Unit", "Create New Record : " + entity.propertyid.ToString() + "-" + entity.propertyname, 0);
            return _mapper.Map<RentalUnit>(entity);
        }

        public async Task UpdateRentalUnit(RentalUnit requestDto)
        {
            var entity = await GetRentalUnitById(requestDto.propertyid);
            entity.propertyname = requestDto.propertyname;
            entity.area = requestDto.area;
            entity.monthlyrent = requestDto.monthlyrent;
            entity.status = requestDto.status;
            if (entity.status != RentalPropertyStatus.Occupied)
                entity.status = requestDto.status;
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(_currentUserService.companyid, _currentUserService.employeeid, AppModule.Projects, "Update Rental Unit", "Update Record : " + entity.propertyid.ToString() + "-" + entity.propertyname, 0);
        }
        private async Task<Entities.RentalProperty> GetRentalUnitById(long id)
        {
            return await _dataContext.rentalProperties.Include(x => x.project).FirstOrDefaultAsync(x => x.propertyid == id) ?? throw new ObjectNotFoundException();
        }
        public async Task<RentalUnit> GetRentalUnitByIdAsync(long id)
        {
            var entity = await GetRentalUnitById(id);
            return _mapper.Map<RentalUnit>(entity);
        }

        public async Task<ICollection<RentalUnit>> GetRentalUnits(long projectID, string search)
        {
            var result = await _dataContext.rentalProperties.Include(x => x.project).Where(x => x.projectid == projectID
                && x.project.companyid == _currentUserService.companyid && (x.propertyname.Contains(search) || x.area.Contains(search)))
                .OrderBy(x => x.propertyname).ToListAsync();
            return _mapper.Map<ICollection<RentalUnit>>(result);
        }

        public async Task<ICollection<RentalUnit>> GetAvailableRentalUnits(string search)
        {
            var result = await _dataContext.rentalProperties.Include(x => x.project).Where(x => x.project.companyid == _currentUserService.companyid
                && x.status == RentalPropertyStatus.Vacant && (x.propertyname.Contains(search) || x.area.Contains(search)))
                .OrderBy(x => x.project.propertyname).ThenBy(x => x.propertyname).ToListAsync();
            return _mapper.Map<ICollection<RentalUnit>>(result);
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

        public async Task<PaginatedResult<Project>> GetProjects(PaginationRequest request)
        {
            var query = _dataContext.projects.Where(x => x.companyid == _currentUserService.companyid 
                    && (x.projectypeid == ProjectType.Subdivision || x.projectypeid == ProjectType.RawLot)
                    && x.propertyname.Contains(request.Search ?? ""))
                .OrderBy(x => x.propertyname).AsQueryable();
            return await query.AsPaginatedResult<Entities.Project, Project>(request, _mapper);
        }

        public async Task<bool> IsNameUnique(string propertyName)
        {
            return !await _dataContext.projects.Where(x => x.companyid == _currentUserService.companyid
                && x.propertyname == propertyName && (x.projectypeid == ProjectType.Subdivision || x.projectypeid == ProjectType.RawLot)).AnyAsync();
        }
    }
}
