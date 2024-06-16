using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Keyless
{
    [Keyless]
    public class RentalAccountHistory
    {
        public DateTime? paymentdate { get; set; }
        public string refno { get; set; } = string.Empty;
        public string particular { get; set; } = string.Empty;
        public decimal interest { get; set; }
        public decimal debit { get; set; }
        public decimal credit { get; set; }
        public decimal runningbalance { get; set; }
        public DateTime? chargedate { get; set; }
        public int seq { get; set; }
        public long chargeid { get; set; }
    }
}
