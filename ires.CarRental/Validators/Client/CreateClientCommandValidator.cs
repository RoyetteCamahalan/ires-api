using FluentValidation;
using ires.Application.Commands.Client;
using ires.Domain.Contracts;

namespace ires.Application.Validators.Client
{
    public sealed class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator(IClientService _ClientService)
        {
            RuleFor(x => x.fname).MustAsync(async (request, fname, _) =>
            {
                return await _ClientService.IsClientNameUnique(request.lname, fname);
            }).WithMessage("Client name already exist");
        }
    }
}
