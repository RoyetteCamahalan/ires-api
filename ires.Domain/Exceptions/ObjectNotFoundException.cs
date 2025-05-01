using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Domain.Exceptions
{
    public class ObjectNotFoundException(string message = "Unable to find object") : Exception(message)
    {
    }
}
