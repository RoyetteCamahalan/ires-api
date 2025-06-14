using FluentValidation;
using ires.Core.Commands.Lot;
using ires.Domain.Contracts;

namespace ires.Core.Validations.Lot
{
    public sealed class CreateLotCommandValidator : AbstractValidator<CreateLotCommand>
    {
        public CreateLotCommandValidator(ILotService _lotService)
        {
            RuleFor(x => x.name).MustAsync(async (data, name, _) =>
            {
                return await _lotService.IsNameUnique(data.propertyid, data.name);
            }).WithMessage("Name already exist in the current project");

            RuleFor(x => x.commissionableamount).Must((model, commissionableamount) => commissionableamount <= model.default_price)
                .WithMessage("Commissionamount must not be greater than the total price.");

            RuleFor(x => x.compercentage).InclusiveBetween(0, 100).WithMessage("Commission percentage must be from 0-100");

            RuleFor(x => x.comatdown).Must((model, comatdown) => comatdown <= model.commissionableamount * (model.compercentage / 100))
                .WithMessage("Commission at downpayment must not be greater than the total commission.");
        }
    }
}
