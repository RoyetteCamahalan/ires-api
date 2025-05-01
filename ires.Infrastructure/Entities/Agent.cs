using ires.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("agents")]
    public class Agent : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id {  get; set; }
        public Guid guid { get; set; }
        public int companyid { get; set; }
        [MaxLength(250)]
        public string firstname { get; set; } = string.Empty;
        [MaxLength(250)]
        public string lastname { get; set; } = string.Empty;
        [MaxLength(250)]
        public string contactno {  get; set; } = string.Empty;
        [MaxLength(500)]
        public string address { get; set; } = string.Empty;
        [MaxLength(100)]
        public string email { get; set; } = string.Empty;
        [MaxLength(250)]
        public string tinnumber { get; set; } = string.Empty;
        public bool isactive { get; set; } = true;
        public long? upline_id { get; set; }
        public Agent? upline { get; set; }
    }
}
