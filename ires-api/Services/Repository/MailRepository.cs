using ires_api.DTO;
using ires_api.Services.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace ires_api.Services.Repository
{
    public class MailRepository : IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogService _logService;

        public MailRepository(IConfiguration configuration, ILogService logService)
        {
            _configuration = configuration;
            _logService = logService;
        }

        public string GetPublicEmail()
        {
            return _configuration["MailSettings:Info:Mail"];
        }

        public bool SendEmailAsync(string subject, List<string> mailTo, string body, bool isHTML = false)
        {
            try
            {
                var mailSetting = new MailSetting(_configuration, "NoReply");
                var mail = new MimeMessage();
                mail.From.Add(MailboxAddress.Parse(mailSetting.Mail));
                foreach (string receiver in mailTo)
                {
                    mail.To.Add(MailboxAddress.Parse(receiver));
                }
                mail.Subject = subject;
                if (isHTML)
                    mail.Body = new TextPart(TextFormat.Html) { Text = body };
                else
                    mail.Body = new TextPart(TextFormat.Plain) { Text = body };

                Task.Run(async () =>
                {
                    using var smtp = new SmtpClient();
                    await smtp.ConnectAsync(mailSetting.Host, mailSetting.Port, SecureSocketOptions.None);
                    await smtp.AuthenticateAsync(mailSetting.Mail, mailSetting.Password);
                    await smtp.SendAsync(mail);
                    await smtp.DisconnectAsync(true);
                });

            }
            catch (Exception ex)
            {
                _logService.SaveLog(0, 0, 0, "mail error", ex.Message, 0);
                return false;
            }
            return true;
        }
    }
}
