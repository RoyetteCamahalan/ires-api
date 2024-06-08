using ires_api.Enumerations;

namespace ires_api.DTO.Project
{
    public class RentalProjectRequestDto
    {
        public long propertyid { get; set; }
        public int companyid { get; set; }
        public string propertyname { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public ProjectType projectypeid { get; set; }
    }
}
