namespace ires_api.Services.Interface
{
    public interface IMailService
    {
        public bool SendEmail(string subject, List<string> mailTo, string body, bool isHTML = false);
        public string GetPublicEmail();
    }
}
