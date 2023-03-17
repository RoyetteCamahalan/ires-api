using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires_api.Models
{
    [Table("employees")]
    public class Employee
    {
        [Key]
        public long employeeid { get; set; }
        public int companyid { get; set; }
        public string firstname { get; set; } = String.Empty;
        public string lastname { get; set; } = String.Empty;
        public string middlename { get; set; } = String.Empty;
        public string gender { get; set; } = String.Empty;
        public string? mobileno { get; set; } 
        public string? email { get; set; }
        public bool? isactive { get; set; }
        public bool? isappsysadmin { get; set; }
        public string? username { get; set; }
        public string? userpass { get; set; }
        public string? designation { get; set; }
        public long? createdbyid { get; set; }
        public DateTime? datecreated { get; set; }

        public Company? company { get; set; }
    }
}
