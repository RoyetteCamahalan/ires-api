using FluentValidation;
using ires.Core.Commands.Agent;
using ires.Domain.Contracts;
using ires.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ires.Domain.DTO.Payment.PayMongoResponseDto;

namespace ires.Core.Validations.Agent
{
    public sealed class UpdateAgentCommandValidator : AbstractValidator<UpdateAgentCommand>
    {
        public UpdateAgentCommandValidator(IAgentService _agentService)
        {
            RuleFor(x => x.firstname).MustAsync(async (data, firstname, _) =>
            {
                return await _agentService.IsNameUnique(data.guid, firstname, data.lastname);
            }).WithMessage("Name already exist");
            RuleFor(x => x.upline_id).MustAsync(async (data, upline_id, _) =>
            {
                var agent = await _agentService.FindAgentByGuid(data.guid);
                if(agent != null && upline_id > 0)
                    return await isValidUplineAsync(_agentService, agent.id, upline_id);
                return true;
            }).WithMessage("Invalid Upline, reference loop detected");
        }
        private async Task<bool> isValidUplineAsync(IAgentService _agentService, long refID, long? upline_id)
        {
            if (refID == upline_id)
                return false;
            var agent = await _agentService.FindAgentByID(upline_id ?? 0);
            if (agent != null && agent.upline_id > 0)
                return await isValidUplineAsync(_agentService, refID, agent.upline_id);
            return true;
        }
    }
}
