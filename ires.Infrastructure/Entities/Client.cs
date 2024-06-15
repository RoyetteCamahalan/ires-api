using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("customer")]
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long custid { get; set; }
        public int companyid { get; set; }
        public string lname { get; set; } = string.Empty;
        public string fname { get; set; } = string.Empty;
        public string mname { get; set; } = string.Empty;
        public DateTime? birthdate { get; set; }
        public string address { get; set; } = string.Empty;
        public string contactno { get; set; } = string.Empty;
        public string tinnumber { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public long createdbyid { get; set; }
        public DateTime? datecreated { get; set; }
        public long updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }
        public List<Survey> surveys { get; set; } = new List<Survey>();
        public List<Payment> payments { get; set; } = new List<Payment>();

    }
}
