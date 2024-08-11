using AutoMapper;
using ires.Application.Queries.Client;
using ires.Application.Queries.RentalContract;
using ires.Application.ViewModels;
using ires.Domain.Common;
using ires.Domain.Contracts;
using MediatR;

namespace ires.Application.QueryHandlers
{
    public class ClientQueryHandler(IClientService _ClientService, IMapper _mapper) :
        IRequestHandler<GetClientByIdQuery, ClientViewModel>,
        IRequestHandler<GetAllClientsQuery, PaginatedResult<ClientViewModel>>
    {
        public async Task<PaginatedResult<ClientViewModel>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<PaginatedResult<ClientViewModel>>(await _ClientService.GetClients(request.data));
        }

        public async Task<ClientViewModel> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ClientViewModel>(await _ClientService.GetByID(request.id));
        }
    }
}
