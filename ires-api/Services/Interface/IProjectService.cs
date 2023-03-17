using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IProjectService
    {
        public Project Create(ProjectRequestDto projectRequest);

        public Project Update(ProjectRequestDto projectRequest);
        public Project GetProjectByName(string name);
        public Project GetProjectById(long id);
        public List<Project> GetProjectByCompany(int companyid);
    }
}
