using ires.Application.ViewModels;
using MediatR;

namespace ires.Application.Queries.Client
{
    public record GetClientByIdQuery(long id) : IRequest<ClientViewModel>
    {
    }
}
