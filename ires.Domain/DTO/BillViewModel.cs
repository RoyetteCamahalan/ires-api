using ires.Domain.Enumerations;

namespace ires.Domain.DTO
{
    public class BillViewModel
    {
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
    }
}
