using ires_api.Models;

namespace ires_api.DTO.Payment
{
    public class PayMongoRequestDto
    {
        public PayMongoRequestDto(Bill bill, IConfiguration configuration)
        {
            attributes = new Attributes();
            attributes.line_items.Add(new LineItem
            {
                amount = Convert.ToInt32(bill.amount * 100),
                description = bill.particular,
                name = "Subscription",
                quantity = 1
            });
            attributes.success_url = configuration["PayMongo:successURL"].ToString() + bill.id.ToString();
            attributes.cancel_url = configuration["PayMongo:cancelURL"].ToString();
            attributes.statement_descriptor = "HexaByt Payment";
            attributes.reference_number = bill.id.ToString();
            attributes.description = bill.particular;
        }
        public Attributes? attributes { get; set; }
    }
    public class Attributes
    {
        public List<LineItem> line_items { get; set; } = new List<LineItem>();
        public List<string> payment_method_types { get; set; } = new List<string> { "card", "gcash", "grab_pay", "paymaya" };
        public bool send_email_receipt { get; set; } = true;
        public bool show_description { get; set; } = true;
        public bool show_line_items { get; set; } = false;
        public string success_url { get; set; } = string.Empty;
        public string cancel_url { get; set; } = string.Empty;
        public string statement_descriptor { get; set; } = string.Empty;
        public string reference_number { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }
    public class LineItem
    {
        public string currency { get; set; } = "PHP";
        public int amount { get; set; }
        public string description { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public int quantity { get; set; }
    }
}
