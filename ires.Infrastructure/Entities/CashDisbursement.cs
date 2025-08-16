using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("pettycashdisbursement")]
    public class CashDisbursement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long disbursementid { get; set; }
        public int companyid { get; set; }
        public long accountid { get; set; }
        public DateTime? refdate { get; set; }
        public string refno { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public string remarks { get; set; } = string.Empty;
        public DisbursementTransType transtype { get; set; }
        public long? refaccountid { get; set; }
        public long refdisbursementid { get; set; }
        public DisbursementStatus status { get; set; }
        public long createdbyid { get; set; }
        public DateTime? datecreated { get; set; }
        public long updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }
        public long? refpaymentid { get; set; }
        public Office? office { get; set; }
        public Office? refOffice { get; set; }
    }
}
