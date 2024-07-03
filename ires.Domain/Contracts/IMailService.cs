namespace ires.Domain.Contracts
{
    public interface IMailService
    {
        public bool SendEmailAsync(string subject, List<string> mailTo, string body, bool isHTML = false);
        public bool SendEmailAsync(string subject, List<string> mailTo, string body, List<string> attachmentPaths, bool isHTML = false);
        public string GetPublicEmail();
    }
}
