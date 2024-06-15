using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("cars")]
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public int companyid { get; set; }
        public string name { get; set; } = string.Empty;
        public int typeid { get; set; }
        public string platenumber { get; set; } = string.Empty;
        public string model { get; set; } = string.Empty;
        public string year { get; set; } = string.Empty;
        public int status { get; set; }
        public bool isactive { get; set; }
        public long createdbyid { get; set; }
        public DateTime datecreated { get; set; }
        public long updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }

        public CarType carType { get; set; } = new CarType();
    }
}
