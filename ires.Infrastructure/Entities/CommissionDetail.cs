using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Infrastructure.Entities
{
    [Table("commissiondetails")]
    public class CommissionDetail : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public long contractid { get; set; }
        public long agentid { get; set; }
        public decimal compercentage { get; set; }
        public decimal referralpercentage { get; set; }
        public bool isagent { get; set; }

        public Agent? agent { get; set; }
    }
}
