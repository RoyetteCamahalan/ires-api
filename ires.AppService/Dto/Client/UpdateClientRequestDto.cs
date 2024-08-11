using System.ComponentModel.DataAnnotations;

namespace ires.AppService.Dto.Client
{
    public class UpdateClientRequestDto
    {
        [Required]
        public long custid { get; set; }
        [Required]
        public string lname { get; set; } = string.Empty;
        [Required]
        public string fname { get; set; } = string.Empty;
        public string mname { get; set; } = string.Empty;
        public DateTime? birthdate { get; set; }
        public string address { get; set; } = string.Empty;
        public string contactno { get; set; } = string.Empty;
        public string tinnumber { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
    }
}
