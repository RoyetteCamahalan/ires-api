using ires_api.Enumerations;

namespace ires_api.DTO.Payment
{
    public class PayableRequestDto
    {
        public AppModule payableType { get; set; }
        public long payableID { get; set; }
        public string description { get; set; } = string.Empty;
        public decimal grossAmount { get; set; }
        public decimal balance { get; set; }
        public decimal paymentAmount { get; set; }
    }
}
