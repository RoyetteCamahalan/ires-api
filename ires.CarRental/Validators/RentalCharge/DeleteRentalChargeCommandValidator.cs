using FluentValidation;
using ires.Application.Commands.RentalCharge;
using ires.Domain.Contracts;

namespace ires.Application.Validators.RentalCharge
{
    public sealed class DeleteRentalChargeCommandValidator : AbstractValidator<DeleteRentalChargeCommand>
    {
        public DeleteRentalChargeCommandValidator(IRentalService _rentalService)
        {
            RuleFor(x => x.chargeid).MustAsync(async (request, chargeid, _) =>
            {
                return !await _rentalService.RentalChageHasPayment(chargeid);
            }).WithMessage("There is a payment for this charge, please void payment first!");
        }
    }
}
