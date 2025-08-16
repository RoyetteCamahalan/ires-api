using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Infrastructure.Entities
{
    [Table("companysettings")]
    public class CompanySetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int companyid { get; set; }
        public long? autocashinaccountid_survey { get; set; }

        [ForeignKey(nameof(autocashinaccountid_survey))]
        public Office? office { get; set; }
    }
}
