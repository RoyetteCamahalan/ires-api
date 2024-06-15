using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("logs")]
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public int companyid { get; set; }
        public AppModule moduleid { get; set; }
        public string logtitle { get; set; } = string.Empty;
        public string logAction { get; set; } = string.Empty;
        public DateTime? logdate { get; set; }
        public long employeeid { get; set; }
        public int withadmin { get; set; }
    }
}
