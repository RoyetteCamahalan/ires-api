using MediatR;

namespace ires.Application.Commands.Client
{
    public record UpdateClientCommand(
        long custid,
        string lname,
        string fname,
        string mname,
        DateTime? birthdate,
        string address,
        string contactno,
        string tinnumber,
        string email) : IRequest
    {
    }
}
