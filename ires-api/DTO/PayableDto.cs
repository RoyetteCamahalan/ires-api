namespace ires_api.DTO
{
    public class PayableDto
    {
        public int payableType { get; set; } //See Constants.AppModules
        public long payableID { get; set; }
        public string description { get; set; } = string.Empty;
        public decimal grossAmount { get; set; }
        public decimal balance { get; set; }
        public decimal paymentAmount { get; set; }
    }
}
