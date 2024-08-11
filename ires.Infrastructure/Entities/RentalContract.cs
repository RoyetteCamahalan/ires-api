using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("rentalcontracts")]
    public class RentalContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long contractid { get; set; }
        public int companyid { get; set; }
        public long contractno { get; set; }
        public long custid { get; set; }
        public DateTime contractdate { get; set; }
        public decimal montlyrent { get; set; }
        public decimal deposit { get; set; }
        public int term { get; set; }
        public RentStatus status { get; set; }
        public decimal totalbalance { get; set; }
        public int noofmonthdeposit { get; set; }
        public int noofmonthadvance { get; set; }
        public decimal advancerent { get; set; }
        public decimal ewtpercentage { get; set; }
        public decimal monthlypenalty { get; set; }
        public int penaltyextension { get; set; }
        public string remarks { get; set; } = string.Empty;
        public int billingsched { get; set; } = 1;
        public long createdbyid { get; set; }
        public DateTime? datecreated { get; set; }
        public long updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }
        public DateTime? dateterminated { get; set; }

        public Client? client { get; set; }
        public List<RentalContractDetail> rentalContractDetails { get; set; } = [];
    }
}
