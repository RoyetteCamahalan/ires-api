using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace ires.AppService.Dto.Car
{
    public class CreateCarRequestDto
    {
        [Required]
        public string name { get; set; } = string.Empty;
        public int typeid { get; set; }
        [Required]
        public string platenumber { get; set; } = string.Empty;
        [Required]
        public string model { get; set; } = string.Empty;
        [Required]
        public string year { get; set; } = string.Empty;
        public CarStatus status { get; set; }
    }
}
