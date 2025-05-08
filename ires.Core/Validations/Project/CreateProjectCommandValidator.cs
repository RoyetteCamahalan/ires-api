















using FluentValidation;
using ires.Core.Commands.Agent;
using ires.Core.Commands.Project;
using ires.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.Validations.Project
{
    public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator(IProjectService _projectService)
        {
            RuleFor(x => x.propertyname).MustAsync(async (data, propertyname, _) =>
            {
                return await _projectService.IsNameUnique(propertyname);
            }).WithMessage("Name already exist");
        }
    }
}
