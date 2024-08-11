using ires.Application.ViewModels;
using ires.Domain.Common;
using MediatR;

namespace ires.Application.Queries.RentalContract
{
    public record GetAllRentalContractsQuery(PaginationRequest data) : IRequest<PaginatedResult<RentalContractViewModel>>
    {
    }
}
