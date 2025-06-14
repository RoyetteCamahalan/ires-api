using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ires.Domain.Entities
{
    public class Contract : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long contractid { get; set; }
        public Guid guid { get; set; }
        public int companyid { get; set; }
        public long contractno { get; set; }
        public long custid { get; set; }
        public DateTime contractdate { get; set; }
        public int term { get; set; }
        public int billingsched { get; set; } = 1;
        public decimal monthlypenalty { get; set; }
        public int penaltyextension { get; set; }
        public int commissionterm { get; set; }
        public string remarks { get; set; } = string.Empty;

        public Client? client { get; set; }
        public List<ContractDetail> contractDetails { get; set; } = [];
    }
}
