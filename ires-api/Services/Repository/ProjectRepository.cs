using AutoMapper;
using ires_api.Data;
using ires_api.DTO.Project;
using ires_api.Models;
using ires_api.Services.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ires_api.Services.Repository
{
    public class ProjectRepository : IProjectService
    {
        private readonly DataContext _dataContext;
        private readonly Mapper _mapper;

        public ProjectRepository(DataContext dataContext, Mapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public Project Create(ProjectRequestDto projectRequest)
        {
            Project project = _mapper.Map<Project>(projectRequest);
            _dataContext.projects.Add(project);
            _dataContext.SaveChanges();
            return project;
        }
        public List<Project> GetProjectByCompany(int companyid)
        {
            return _dataContext.projects.Where(x => x.companyid == companyid).ToList();
        }

        public Project GetProjectById(long id)
        {
            return _dataContext.projects.Find(id);
        }

        public Project GetProjectByName(string name)
        {
            return _dataContext.projects.Where(x => x.propertyname == name).FirstOrDefault();
        }

        public Project Update(ProjectRequestDto projectRequest)
        {
            Project project = this.GetProjectById(projectRequest.propertyid);
            if(project != null)
            {
                _dataContext.Entry(project).CurrentValues.SetValues(projectRequest);
                _dataContext.SaveChanges();
            }
            return project;
        }
    }
}
