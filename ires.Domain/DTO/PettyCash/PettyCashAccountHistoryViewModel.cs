namespace ires.Domain.DTO.PettyCash
{
    public class PettyCashAccountHistoryViewModel
    {
        public long transid { get; set; }
        public int transtype { get; set; }
        public string refno { get; set; } = string.Empty;
        public string particular { get; set; } = string.Empty;
        public DateTime? transdate { get; set; }
        public DateTime? actualdate { get; set; }
        public decimal amount { get; set; }
        public decimal debit { get; set; }
        public decimal credit { get; set; }
        public string remarks { get; set; } = string.Empty;
        public decimal runningbalance { get; set; }
    }
}
