using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires_api.Models
{
    [Table("vendors")]
    public class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long vendorid { get; set; }
        public int companyid { get; set; }
        public string vendorname { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string contactno { get; set; } = string.Empty;
        public string tinno { get; set; } = string.Empty;
        public bool isactive { get; set; }
        public long createdbyid { get; set; }
        public DateTime? datecreated { get; set; }
        public long updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }

    }
}
