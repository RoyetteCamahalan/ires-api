using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires_api.Models
{
    [Table("applicationmodules")]
    public class ApplicationModule
    {
        [Key]
        public int moduleid { get; set; }
        public string modulename { get; set; } = string.Empty;
        public bool? isactive { get; set; }
    }
}

