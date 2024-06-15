using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("otherfees")]
    public class OtherFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public decimal price { get; set; }
        public bool isactive { get; set; }
        public long? createdby { get; set; }
        public DateTime? datecreated { get; set; }
        public long? updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }
        public ICollection<OtherCharge>? otherCharges { get; set; }
    }
}
