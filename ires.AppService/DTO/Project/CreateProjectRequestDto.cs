using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace ires.AppService.DTO.Project
{
    public class CreateProjectRequestDto
    {
        [Required]
        public string propertyname { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string alias { get; set; } = string.Empty;
        public decimal area { get; set; }
        public ProjectType projectypeid { get; set; }
    }
}
