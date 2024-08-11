using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ires.Infrastructure.Repositories
{
    public class MailRepository(IConfiguration _configuration, ILogService _logService) : IMailService
    {

        public string GetPublicEmail()
        {
            return _configuration["MailSettings:Info:Mail"];
        }

        public bool SendEmailAsync(string subject, List<string> mailTo, string body, bool isHTML = false)
        {
            return SendEmailAsync(subject, mailTo, body, [], isHTML);
        }

        public bool SendEmailAsync(string subject, List<string> mailTo, string body, List<string> attachmentPaths, bool isHTML = false)
        {
            try
            {
                var mailSetting = new MailSetting(_configuration, Constants.MailSection.noReply);
                var mail = new MimeMessage();
                mail.From.Add(MailboxAddress.Parse(mailSetting.Mail));
                foreach (string receiver in mailTo)
                {
                    mail.To.Add(MailboxAddress.Parse(receiver));
                }
                mail.Subject = subject;

                var builder = new BodyBuilder();
                if (isHTML)
                    builder.HtmlBody = body;
                else
                    builder.TextBody = body;

                foreach (var path in attachmentPaths)
                {
                    var fileName = Path.GetFileName(path);
                    var attachment = new MimePart(MimeTypes.GetMimeType(fileName))
                    {
                        Content = new MimeContent(File.OpenRead(path)),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(path)
                    };

                    builder.Attachments.Add(attachment);
                }
                mail.Body = builder.ToMessageBody();

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
                Task.Run(async () =>
                {
                    await _logService.SaveLogAsync(0, 0, 0, "mail error", ex.Message, 0);
                });
                return false;
            }
            return true;
        }
    }
}
