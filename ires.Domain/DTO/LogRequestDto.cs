using ires.Domain.Enumerations;

namespace ires.Domain.DTO
{
    public class LogRequestDto
    {
        public int companyid { get; set; }
        public AppModule moduleid { get; set; }
        public string logtitle { get; set; } = string.Empty;
        public string logAction { get; set; } = string.Empty;
        public long employeeid { get; set; }
    }
}
