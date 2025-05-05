using AutoMapper;
using ires.Core.Queries.Agent;
using ires.Core.Queries.Project;
using ires.Core.ViewModels;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.Employee;
using ires.Infrastructure.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.QueryHandlers
{
    public class ProjectQueryHandler(IProjectService _projectService, IMapper _mapper) :
        IRequestHandler<GetProjectsQuery, PaginatedResult<ProjectViewModel>>,
        IRequestHandler<GetProjectbyIdQuery, ProjectViewModel>
    {

        public async Task<PaginatedResult<ProjectViewModel>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var data = await _projectService.GetProjects(request.data);

            return _mapper.Map<PaginatedResult<ProjectViewModel>>(data);
        }

        public async Task<ProjectViewModel> Handle(GetProjectbyIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ProjectViewModel>(await _projectService.GetProjectByIdAsync(request.id));
        }
    }
}
