using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Domain.Contracts
{
    public interface ICurrentUserService
    {
        long employeeid { get; }
        int companyid { get; }
    }
}
