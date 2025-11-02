using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("userpermissions")]
    public class UserPermission
    {
        public long EmployeeId { get; set; }
        public int PermissionId { get; set; }
        public Employee? Employee { get; set; }
        public virtual Permission? Permission { get; set; }
    }
}
