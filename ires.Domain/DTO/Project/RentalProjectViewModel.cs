namespace ires.Domain.DTO.Project
{
    public class RentalProjectViewModel
    {
        public long propertyid { get; set; }
        public int companyid { get; set; }
        public string propertyname { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public int projectypeid { get; set; }
        public int noofunits { get; set; }
        public int noofoccupiedunits { get; set; }
    }
}
