using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("maintenancetypes")]
    public class MaintenanceType
    {
        [Key]
        public int id { get; set; }
        [MaxLength(250)]
        public string name { get; set; } = string.Empty;
        public bool isactive { get; set; }
    }
}
