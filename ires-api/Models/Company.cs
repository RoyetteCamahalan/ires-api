using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires_api.Models
{
    [Table("company")]
    public class Company
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
        public string? contactno { get; set; }
        public bool? isactive { get; set; }

        public bool? isverified { get; set; }

        public List<Employee> employees { get; set; } = new List<Employee>();
    }
}
