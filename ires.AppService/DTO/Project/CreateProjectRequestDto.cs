using System.ComponentModel.DataAnnotations;

namespace ires.AppService.DTO.Project
{
    public class CreateProjectRequestDto
    {
        [Required]
        public string propertyname { get; set; } = string.Empty;
        [Required]
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
        public int allow_straight_monthly { get; set; } = 1;
        public decimal withholding { get; set; }
        public int interesttype { get; set; }
        public decimal addoninterestpermonth { get; set; }
    }
}
