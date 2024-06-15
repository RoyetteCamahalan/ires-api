using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("notifications")]
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public long employeeid { get; set; }
        public string details { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public DateTime datecreated { get; set; }
        public bool isread { get; set; }
        public int typeid { get; set; }
    }
}
