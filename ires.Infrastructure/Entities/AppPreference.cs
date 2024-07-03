using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("apppreferences")]
    public class AppPreference
    {
        [Key]
        public AppPrefKey id { get; set; }
        [MaxLength(255)]
        public string name { get; set; } = string.Empty;
        public string value { get; set; } = string.Empty;
    }
}
