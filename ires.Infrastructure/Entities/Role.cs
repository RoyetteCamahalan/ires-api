using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("roles")]
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<RolePermission>? RolePermissions { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }
    }
}
