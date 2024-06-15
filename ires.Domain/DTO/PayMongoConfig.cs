using Microsoft.Extensions.Configuration;

namespace ires.Domain.DTO
{
    public class PayMongoConfig
    {
        public PayMongoConfig(IConfiguration configuration)
        {
            apiURL = configuration["PayMongo:apiURL"] ?? "";
            successURL = configuration["PayMongo:successURL"] ?? "";
            cancelURL = configuration["PayMongo:cancelURL"] ?? "";
            secretKey = configuration["PayMongo:secretKey"] ?? "";
        }
        public PayMongoConfig(string apiURL, string successURL, string cancelURL, string secretKey)
        {
            this.apiURL = apiURL;
            this.successURL = successURL;
            this.cancelURL = cancelURL;
            this.secretKey = secretKey;
        }
        public string apiURL { get; set; } = string.Empty;
        public string successURL { get; set; } = string.Empty;
        public string cancelURL { get; set; } = string.Empty;
        public string secretKey { get; set; } = string.Empty;
    }
}
