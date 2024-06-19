using ires.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Keyless
{
    [Keyless]
    public class Payable
    {
        public AppModule payableType { get; set; }
        public long payableID { get; set; }
        public string description { get; set; } = string.Empty;
        public decimal grossAmount { get; set; }
        public decimal balance { get; set; }
        public decimal paymentAmount { get; set; }
    }
}
