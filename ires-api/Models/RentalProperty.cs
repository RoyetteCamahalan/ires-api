using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires_api.Models
{
    [Table("propertyrentals")]
    public class RentalProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long propertyid { get; set; }

        public long projectid { get; set; }
        public string propertyname { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string contactno { get; set; } = string.Empty;
        public decimal monthlyrent { get; set; } = 0;
        public string alias { get; set; } = string.Empty;

        public Project? project { get; set; }
    }
}
