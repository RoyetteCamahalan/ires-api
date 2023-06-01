using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires_api.Models
{
    [Table("cartypes")]
    public class CarType
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public bool isactive { get; set; }
    }
}
