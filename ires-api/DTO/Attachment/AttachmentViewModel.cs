namespace ires_api.DTO.Attachment
{
    public class AttachmentViewModel
    {
        public long documentid { get; set; }
        public int companyid { get; set; }
        public long invoiceno { get; set; }
        public long lotid { get; set; }
        public string documentname { get; set; } = string.Empty;
        public int filetype { get; set; }
        public bool isdeleted { get; set; }
        public long attachedby { get; set; }
        public DateTime? dateattached { get; set; }
        public int typeid { get; set; }
        public decimal filesize { get; set; }
        public string filename { get; set; } = string.Empty;
    }
}
