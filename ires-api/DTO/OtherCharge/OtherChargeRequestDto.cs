namespace ires_api.DTO.OtherCharge
{
    public class OtherChargeRequestDto
    {
        public long chargeid { get; set; }
        public long surveyid { get; set; }
        public long invoiceno { get; set; }
        public long lotid { get; set; }
        public DateTime chargedate { get; set; }
        public decimal chargeamount { get; set; }
        public decimal interestamount { get; set; }
        public decimal balance { get; set; }
        public long chargetype { get; set; }
        public decimal runningbalance { get; set; }
        public int interestype { get; set; } //See Constants.InterestType
        public decimal interestpercentage { get; set; }
    }
}
