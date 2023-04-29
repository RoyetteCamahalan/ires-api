namespace ires_api.DTO
{
    public class PayMongoResponseDto
    {
        public Data? data { get; set; }
        public class Data
        {
            public string id { get; set; } = string.Empty;
            public Attributes? attributes { get; set; }
        }
        public class Attributes
        {
            public string checkout_url { get; set; } = string.Empty;
            public List<Payments> payments { get; set; } = new List<Payments>();
            public string payment_method_used { get; set; } = string.Empty;
        }
        public class Payments
        {
            public string id { get; set; } = string.Empty;
            public PaymentAttributes? attributes { get; set; }
        }
        public class PaymentAttributes
        {
            public long paid_at { get; set; }
            public string status { get; set; } = string.Empty;
        }
    }
}
