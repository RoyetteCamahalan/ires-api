using System.ComponentModel.DataAnnotations;

namespace ires.AppService.Dto
{
    public class SendMailRequestDto
    {
        [Required]
        public string name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string email { get; set; } = string.Empty;
        [Required]
        public string message { get; set; } = string.Empty;
    }
}
