using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("userprivileges")]
    public class UserPrivilege
    {
        [Key]
        public long userprivid { get; set; }
        public int moduleid { get; set; }
        public long userid { get; set; }
        public bool canadd { get; set; } = false;
        public bool canedit { get; set; } = false;
        public bool canview { get; set; } = false;
        public bool canverify { get; set; } = false;
        public bool canaccess { get; set; } = false;
        public bool canvoid { get; set; } = false;
        public long createdbyid { get; set; }
        public DateTime? datecreated { get; set; }
        public Module? module { get; set; }
    }
}
