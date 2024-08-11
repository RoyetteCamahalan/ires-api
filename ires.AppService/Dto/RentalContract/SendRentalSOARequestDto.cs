using System.ComponentModel.DataAnnotations;

namespace ires.AppService.Dto.RentalContract
{
    public class SendRentalSOARequestDto
    {
        [Required]
        public long id { get; set; }
        [Required, EmailAddress]
        public string email { get; set; } = string.Empty;
    }
}
