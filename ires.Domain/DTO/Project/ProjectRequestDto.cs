using ires.Domain.Enumerations;

namespace ires.Domain.DTO.Project
{
    public class ProjectRequestDto
    {
        public long propertyid { get; set; }
        public string propertyname { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string alias { get; set; } = string.Empty;
        public decimal area { get; set; }
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
    }
}
