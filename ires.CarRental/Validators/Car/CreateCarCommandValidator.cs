using FluentValidation;
using ires.CarRental.Commands.Car;
using ires.Domain.Contracts;

namespace ires.CarRental.Validators.Car
{
    public sealed class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
    {
        public CreateCarCommandValidator(ICarService _carService)
        {
            RuleFor(X => X.platenumber).MustAsync(async (platenumber, _) =>
            {
                return await _carService.IsPlateNumberUnique(0, platenumber);
            }).WithMessage("Car plate number already exist");
        }
    }
}
