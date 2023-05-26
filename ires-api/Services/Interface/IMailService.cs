namespace ires_api.Services.Interface
{
    public interface IMailService
    {
        public bool SendEmailAsync(string subject, List<string> mailTo, string body, bool isHTML = false);
        public string GetPublicEmail();
    }
}
