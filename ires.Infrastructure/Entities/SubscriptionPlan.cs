using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("subscriptionplan")]
    public class SubscriptionPlan
    {
        [Key]
        public int id { get; set; }
        public int moduleid { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public bool isactive { get; set; }
        public decimal storage { get; set; }
        public int surveylimit { get; set; }
        public decimal monthlysubscription { get; set; }
        public List<Company> companies { get; set; } = new List<Company>();
    }
}
