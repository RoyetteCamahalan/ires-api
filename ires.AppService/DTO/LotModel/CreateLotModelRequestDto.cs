using System.ComponentModel.DataAnnotations;

namespace ires.AppService.DTO.LotModel
{
    public class CreateLotModelRequestDto
    {
        [Required]
        public long project_id { get; set; }
        [Required]
        public string name { get; set; } = string.Empty;
    }
}
