namespace ires.Domain.DTO.OtherFee
{
    public class OtherFeeViewModel
    {
        public long id { get; set; }
        public int companyid { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public decimal price { get; set; }
        public bool isactive { get; set; }
        public long createdby { get; set; }
        public DateTime datecreated { get; set; }
        public long? updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }
    }
}
