using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Domain.Enumerations
{
    public enum ContractStatus
    {
        Open = 0,
        Forfeited = 1,
        Paid = 2,
        ReserveOnly = 3,
        Void = 4
    }
}
