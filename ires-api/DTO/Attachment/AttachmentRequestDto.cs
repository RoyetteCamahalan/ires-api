namespace ires_api.DTO.Attachment
{
    public class AttachmentRequestDto
    {
        public int companyid { get; set; }
        public long invoiceno { get; set; }
        public long lotid { get; set; }
        public string documentname { get; set; } = string.Empty;
        public long attachedby { get; set; }
        public int typeid { get; set; }
        public IFormFile? formFile { get; set; }
    }
}
