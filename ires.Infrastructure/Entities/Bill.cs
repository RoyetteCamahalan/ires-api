using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("bill")]
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public int companyid { get; set; }
        public DateTime? billdate { get; set; }
        public string particular { get; set; } = string.Empty;
        public DateTime? datefrom { get; set; }
        public DateTime? dateend { get; set; }
        public DateTime? duedate { get; set; }
        public decimal balance { get; set; }
        public decimal discount { get; set; }
        public decimal amount { get; set; }
        public BillStatus status { get; set; }
        public string paymentmode { get; set; } = string.Empty;
        public string paymentrefno { get; set; } = string.Empty;
        public string checkouturl { get; set; } = string.Empty;
        public string paymentid { get; set; } = string.Empty;
        public bool issent { get; set; }
        public DateTime? datepaid { get; set; }
        public Company? company { get; set; }
    }
}
