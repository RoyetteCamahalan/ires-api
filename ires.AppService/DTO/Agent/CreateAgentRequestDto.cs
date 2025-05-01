using System.ComponentModel.DataAnnotations;

namespace ires.AppService.DTO.Agent
{
    public class CreateAgentRequestDto
    {
        [Required]
        [MaxLength(250)]
        public string firstname { get; set; } = string.Empty;
        [Required]
        [MaxLength(250)]
        public string lastname { get; set; } = string.Empty;
        [MaxLength(250)]
        public string contactno { get; set; } = string.Empty;
        [MaxLength(500)]
        public string address { get; set; } = string.Empty;
        [MaxLength(100)]
        public string email { get; set; } = string.Empty;
        [MaxLength(250)]
        public string tinnumber { get; set; } = string.Empty;
        public long? upline_id { get; set; }
    }
}
