namespace ires_api.DTO
{
    public class PlanDto
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public decimal storage { get; set; }
        public int surveylimit { get; set; }
        public decimal monthlysubscription { get; set; }
    }
}
