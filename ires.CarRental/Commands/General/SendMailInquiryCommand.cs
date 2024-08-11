using MediatR;

namespace ires.Application.Commands.General
{
    public record SendMailInquiryCommand(
        string name,
        string email,
        string message) : IRequest
    {
    }
}
