using ires.Domain.DTO.Client;
using ires.Domain.Enumerations;

namespace ires.Domain.DTO.RentalContract
{
    public class RentalContractViewModel
    {
        public long contractid { get; set; }
        public int companyid { get; set; }
        public long contractno { get; set; }
        public long custid { get; set; }
        public DateTime contractdate { get; set; }
        public decimal montlyrent { get; set; }
        public decimal deposit { get; set; }
        public int term { get; set; }
        public RentStatus status { get; set; }
        public decimal totalbalance { get; set; }
        public int noofmonthdeposit { get; set; }
        public int noofmonthadvance { get; set; }
        public decimal advancerent { get; set; }
        public decimal ewtpercentage { get; set; }
        public decimal monthlypenalty { get; set; }
        public int penaltyextension { get; set; }
        public int billingsched { get; set; }
        public string remarks { get; set; } = string.Empty;
        public long createdbyid { get; set; }
        public DateTime? datecreated { get; set; }
        public long updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }
        public DateTime? dateterminated { get; set; }
        public string propertyList { get; set; } = string.Empty;

        public ClientViewModel? client { get; set; }
    }
}
