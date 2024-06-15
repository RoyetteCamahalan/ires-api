namespace ires.Domain.DTO
{
    public class SubscriptionPlanViewModel
    {
        public int id { get; set; }
        public int moduleid { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public bool isactive { get; set; }
        public decimal storage { get; set; }
        public int surveylimit { get; set; }
        public decimal monthlysubscription { get; set; }
    }
}
