namespace ires_api.DTO
{
    public class NotificationDto
    {
        public long id { get; set; }
        public string details { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public DateTime datecreated { get; set; }
        public bool isread { get; set; }
        public int typeid { get; set; }
    }
}
