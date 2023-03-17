namespace ires_api.Services.Interface
{
    public interface IMailService
    {
        public void SendEmail(string subject, List<string> mailTo, string body, bool isHTML = false);
    }
}
