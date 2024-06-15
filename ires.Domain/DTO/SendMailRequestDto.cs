namespace ires.Domain.DTO
{
    public class SendMailRequestDto
    {
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
    }
}
