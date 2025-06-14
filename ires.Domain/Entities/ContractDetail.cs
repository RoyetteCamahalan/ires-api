using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Domain.Entities
{
    [Table("contractdetails")]
    public class ContractDetail : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public Guid guid { get; set; }
        public long contractid { get; set; }
        public long lotid { get; set; }
        public decimal price { get; set; }
        public decimal commissionableprice { get; set; }
        public decimal commissionpercentage { get; set; }
        public decimal amortization { get; set; }
        public decimal adcom { get; set; }
        public decimal referralfee { get; set; }
        public ContractStatus status { get; set; }
        public DateTime? dateforfeited { get; set; }
        public string forfeitreason { get; set; } = string.Empty;
        public long? forfeitedbyid { get; set; }
        public bool hasrealtytax { get; set; }
        public decimal totalcapitalgains { get; set; }
        public decimal totaltaxes { get; set; }
        public decimal totalotherfees { get; set; }
        public decimal totallotbalance { get; set; }
        public decimal totaldownpayment { get; set; }
        public bool istitlereleased { get; set; }
        public DateTime? titlereleasedate { get; set; }
        public string titlereleasedby { get; set; } = string.Empty;
        public string titlereceivedby { get; set; } = string.Empty;
        public decimal totalarrears { get; set; }

        public Contract? contract { get; set; }
        public Lot? lot { get; set; }
    }
}
