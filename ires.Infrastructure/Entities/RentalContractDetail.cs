using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("rentalcontractdetails")]
    public class RentalContractDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public long contractid { get; set; }
        public long propertyid { get; set; }
        public long createdbyid { get; set; }
        public DateTime? datecreated { get; set; }
        public DateTime? deleted_at { get; set; }

        public RentalContract? rentalContract { get; set; }
        public RentalProperty? rentalProperty { get; set; }
    }
}
