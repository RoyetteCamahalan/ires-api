using ires.Domain.Enumerations;

namespace ires.Domain.DTO.RentalContract
{
    public class RentalTerminateRequestDto
    {
        public long contractid { get; set; }
        public RentStatus status { get; set; }
        public DateTime? dateterminated { get; set; }
        public long updatedbyid { get; set; }
    }
}
