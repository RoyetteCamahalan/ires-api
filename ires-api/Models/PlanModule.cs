using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires_api.Models
{
    [Table("planmodules")]
    public class PlanModule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public int moduleid { get; set; }
        public int planid { get; set; }
        public bool isactive { get; set; }
        public Module? module { get; set; }
    }
}
