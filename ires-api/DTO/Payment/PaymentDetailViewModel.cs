using ires_api.Enumerations;

namespace ires_api.DTO.Payment
{
    public class PaymentDetailViewModel
    {
        public long paymentdetailid { get; set; }
        public long chargeid { get; set; }
        public long otherfeeid { get; set; }
        public long paymentid { get; set; }
        public decimal balance { get; set; }
        public decimal discount { get; set; }
        public long discount_type { get; set; }
        public decimal amount { get; set; }
        public long defaultchargeid { get; set; }
        public decimal runningbalance { get; set; }
        public long invoiceno { get; set; }
        public long lotid { get; set; }
        public long paymenttype { get; set; }
        public long rentalchargeid { get; set; }
        public long rentalid { get; set; }
        public string remarks_deletion { get; set; } = string.Empty;
        public AppModule payableType { get; set; }
        public long surveyid { get; set; }
        public PaymentViewModel? payment { get; set; }
    }
}
