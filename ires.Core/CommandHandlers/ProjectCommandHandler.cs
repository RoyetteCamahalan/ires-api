using AutoMapper;
using ires.Core.Commands.Project;
using ires.Core.ViewModels;
using ires.Domain.Contracts;
using ires.Domain.Enumerations;
using ires.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.CommandHandlers
{
    public class ProjectCommandHandler(IProjectService _projectService, IMapper _mapper) :
        IRequestHandler<CreateProjectCommand, ProjectViewModel>,
        IRequestHandler<UpdateProjectCommand>
    {
        public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            await _projectService.Update(_mapper.Map<Project>(request));
            return new Unit();
        }

        public async Task<ProjectViewModel> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = _mapper.Map<Project>(request);
            project.projectypeid = ProjectType.Subdivision;
            return _mapper.Map<ProjectViewModel>(await _projectService.Create(project));
        }
    }
}
