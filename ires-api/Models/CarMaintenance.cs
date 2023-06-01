using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires_api.Models
{
    [Table("carmaintenance")]
    public class CarMaintenance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public int companyid { get; set; }
        public long carid { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public int typeid { get; set; }
        public int status { get; set; }
        public string remarks { get; set; } = string.Empty;
        public long createdbyid { get; set; }
        public DateTime datecreated { get; set; }
        public long updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }

        public Car car { get; set; } = new Car();
        public MaintenanceType maintenanceType { get; set; } = new MaintenanceType();
    }
}
