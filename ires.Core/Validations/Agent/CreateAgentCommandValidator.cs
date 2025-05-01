using FluentValidation;
using ires.Core.Commands.Agent;
using ires.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.Validations.Agent
{
    public sealed class CreateAgentCommandValidator : AbstractValidator<CreateAgentCommand>
    {
        public CreateAgentCommandValidator(IAgentService _agentService)
        {
            RuleFor(x => x.firstname).MustAsync(async (data, firstname, _) =>
            {
                return await _agentService.IsNameUnique(Guid.NewGuid(), firstname, data.lastname);
            }).WithMessage("Name already exist");
        }
    }
}
