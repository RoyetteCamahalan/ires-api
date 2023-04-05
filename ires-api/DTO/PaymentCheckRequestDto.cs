namespace ires_api.DTO
{
    public class PaymentCheckRequestDto
    {
        public string checkno { get; set; } = string.Empty;
        public long bankid { get; set; }
        public DateTime? checkdate { get; set; }
        public string accountnumber { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public string memo { get; set; } = string.Empty;
    }
}
