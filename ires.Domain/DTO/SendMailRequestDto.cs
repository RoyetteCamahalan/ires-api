using System.ComponentModel.DataAnnotations;

namespace ires.Domain.DTO
{
    public class SendMailRequestDto
    {
        public long id { get; set; }
        [Required]
        public string name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string email { get; set; } = string.Empty;
        [Required]
        public string message { get; set; } = string.Empty;
    }
}
