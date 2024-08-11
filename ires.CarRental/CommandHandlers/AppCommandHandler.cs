using ires.Application.Commands.General;
using ires.Domain.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace ires.Application.CommandHandlers
{
    public class AppCommandHandler(
        IMailService _mailService,
        ILogService _logService) :
        IRequestHandler<SendMailInquiryCommand>
    {
        public async Task<Unit> Handle(SendMailInquiryCommand request, CancellationToken cancellationToken)
        {
            if (_mailService.SendEmailAsync("Message From: " + request.name, [_mailService.GetPublicEmail()], "Email: " + request.email + " Message: " + request.message))
                throw new Exception("Failed to send email.");
            await _logService.SaveLogAsync(Domain.Enumerations.AppModule.Clients, "Email inquiry send!", JsonConvert.SerializeObject(request));
            return new Unit();
        }
    }
}
