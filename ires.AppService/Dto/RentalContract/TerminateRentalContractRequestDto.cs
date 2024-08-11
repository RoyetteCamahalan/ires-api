using ires.Domain.Enumerations;

namespace ires.AppService.Dto.RentalContract
{
    public class TerminateRentalContractRequestDto
    {
        public long contractid { get; set; }
        public RentStatus status { get; set; }
        public DateTime? dateterminated { get; set; }
    }
}
