using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Infrastructure.Common
{
    public partial class BaseEntity
    {
        public long createdbyid { get; set; }
        public DateTime datecreated { get; set; }
        public long? updatedbyid { get;set; }
        public DateTime? dateupdated { get; set; }
    }
}
