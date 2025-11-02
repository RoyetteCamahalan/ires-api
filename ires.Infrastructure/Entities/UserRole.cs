using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("userroles")]
    public class UserRole
    {
        public int RoleId { get; set; }
        public long EmployeeId { get; set; }

        public Role? Role { get; set; }
        public Employee? Employee { get; set; }
    }
}
