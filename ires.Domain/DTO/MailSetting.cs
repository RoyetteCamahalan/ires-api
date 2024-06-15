using Microsoft.Extensions.Configuration;

namespace ires.Domain.DTO
{
    public class MailSetting
    {
        public MailSetting(IConfiguration configuration, string section)
        {
            Mail = configuration["MailSettings:" + section + ":Mail"] ?? "";
            DisplayName = configuration["MailSettings:" + section + ":DisplayName"] ?? "";
            Password = configuration["MailSettings:" + section + ":Password"] ?? "";
            Host = configuration["MailSettings:" + section + ":Host"] ?? "";
            Port = Convert.ToInt32(configuration["MailSettings:" + section + ":Port"] ?? "");
        }
        public string Mail { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
