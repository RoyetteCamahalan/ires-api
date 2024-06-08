using System.ComponentModel.DataAnnotations;

namespace ires_api.DTO.RentalContractDetail
{
    public class RentalContractDetailRequestDto
    {
        public long id { get; set; }
        [Required]
        public long contractid { get; set; }
        [Required]
        public long propertyid { get; set; }
    }
}
