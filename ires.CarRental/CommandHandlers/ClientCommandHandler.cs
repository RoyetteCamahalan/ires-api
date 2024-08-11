using AutoMapper;
using ires.Application.Commands.Client;
using ires.Application.ViewModels;
using ires.Domain.Contracts;
using ires.Domain.Models;
using MediatR;

namespace ires.Application.CommandHandlers
{
    public class ClientCommandHandler(IClientService _ClientService, IMapper _mapper) :
        IRequestHandler<CreateClientCommand, ClientViewModel>,
        IRequestHandler<UpdateClientCommand>
    {
        public async Task<Unit> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            await _ClientService.Update(_mapper.Map<Client>(request));
            return new Unit();
        }

        public async Task<ClientViewModel> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var result = await _ClientService.Create(_mapper.Map<Client>(request));
            return _mapper.Map<ClientViewModel>(result);
        }
    }
}
