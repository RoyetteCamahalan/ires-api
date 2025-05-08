using System.ComponentModel.DataAnnotations;

namespace ires.AppService.DTO.LotModel
{
    public class UpdateLotModelRequestDto
    {
        [Required]
        public long id { get; set; }
        [Required]
        public string name { get; set; } = string.Empty;
    }
}
