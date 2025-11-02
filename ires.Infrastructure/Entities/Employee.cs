using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("employees")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long employeeid { get; set; }
        public int companyid { get; set; }
        public string firstname { get; set; } = String.Empty;
        public string lastname { get; set; } = String.Empty;
        public string middlename { get; set; } = String.Empty;
        public string gender { get; set; } = String.Empty;
        public string? mobileno { get; set; }
        public string? email { get; set; }
        public bool isactive { get; set; }
        public bool isappsysadmin { get; set; }
        public string? username { get; set; }
        public string? userpass { get; set; }
        public string? designation { get; set; }
        public long? createdbyid { get; set; }
        public DateTime? datecreated { get; set; }
        public long? updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }
        public string passwordresettoken { get; set; } = string.Empty;

        public Company? company { get; set; }

        public virtual ICollection<UserPermission>? UserPermissions { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }
    }
}
