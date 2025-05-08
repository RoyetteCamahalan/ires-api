using FluentValidation;
using ires.Core.Commands.Agent;
using ires.Core.Commands.LotModel;
using ires.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.Validations.LotModel
{
    public sealed class CreateLotModelCommandValidator : AbstractValidator<CreateLotModelCommand>
    {
        public CreateLotModelCommandValidator(ILotModelService _lotModelService)
        {
            RuleFor(x => x.name).MustAsync(async (data, name, _) =>
            {
                return await _lotModelService.IsNameUnique(data.project_id, data.name);
            }).WithMessage("Name already exist");
        }
    }
}
