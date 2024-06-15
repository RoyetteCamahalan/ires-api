using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("property")]
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long propertyid { get; set; }
        public int companyid { get; set; }
        public string propertyname { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string alias { get; set; } = string.Empty;
        public decimal area { get; set; }
        public Boolean isjointventure { get; set; }
        public int computationtype { get; set; }
        public decimal defaultcommission { get; set; }
        public decimal com_percentage { get; set; }
        public decimal compercentageoverterm { get; set; }
        public int paymentterm { get; set; }
        public decimal interest { get; set; }
        public int commissionterm { get; set; }
        public int paymentextension { get; set; }
        public int allow_straight_monthly { get; set; }
        public decimal withholding { get; set; }
        public int interesttype { get; set; }
        public decimal addoninterestpermonth { get; set; }
        public ProjectType projectypeid { get; set; }
        public List<RentalProperty> rentalProperties { get; set; } = new List<RentalProperty>();

    }
}
