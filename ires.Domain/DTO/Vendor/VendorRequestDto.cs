namespace ires.Domain.DTO.Vendor
{
    public class VendorRequestDto
    {
        public long vendorid { get; set; }
        public int companyid { get; set; }
        public string vendorname { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string contactno { get; set; } = string.Empty;
        public string tinno { get; set; } = string.Empty;
        public bool isactive { get; set; }
        public long createdbyid { get; set; }
        public long updatedbyid { get; set; }
    }
}
