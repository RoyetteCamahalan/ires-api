namespace ires.Domain.DTO.RentalUnit
{
    public class RentalUnitRequestDto
    {
        public long propertyid { get; set; }
        public int companyid { get; set; }
        public long projectid { get; set; }
        public string propertyname { get; set; } = string.Empty;
        public string area { get; set; } = string.Empty;
        public decimal monthlyrent { get; set; } = 0;
        public bool isactive { get; set; }
        public long createdbyid { get; set; }
        public long updatedbyid { get; set; }
    }
}
