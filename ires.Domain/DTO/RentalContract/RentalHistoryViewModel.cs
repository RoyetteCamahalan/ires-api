namespace ires.Domain.DTO.RentalContract
{
    public class RentalHistoryViewModel
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
