using ires.Application.ViewModels;
using MediatR;

namespace ires.Application.Queries.RentalContract
{
    public record GenerateRentalSOAQuery(long id) : IRequest<FileDataViewModel>
    {
    }
}
