using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("company")]
    public class Company
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
        public string? contactno { get; set; }
        public bool? isactive { get; set; }

        public bool isverified { get; set; }
        public int planid { get; set; }
        public DateTime? subscriptionexpiry { get; set; }
        public decimal storage { get; set; }
        public int surveylimit { get; set; }
        public int billingcycle { get; set; }
        public decimal amount { get; set; }

        public List<Employee> employees { get; set; } = new List<Employee>();
        public SubscriptionPlan? subscriptionPlan { get; set; }
    }
}
