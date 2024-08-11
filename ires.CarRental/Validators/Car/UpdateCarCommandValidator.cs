using FluentValidation;
using ires.Application.Commands.Car;
using ires.Domain.Contracts;

namespace ires.Application.Validators.Car
{
    public sealed class UpdateCarCommandValidator : AbstractValidator<UpdateCarCommand>
    {
        public UpdateCarCommandValidator(ICarService _carService)
        {
            RuleFor(X => X.platenumber).MustAsync(async (command, platenumber, _) =>
            {
                return await _carService.IsPlateNumberUnique(command.id, platenumber);
            }).WithMessage("Car plate number already exist");
        }
    }
}
