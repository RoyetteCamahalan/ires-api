using ires_api.Models;

namespace ires_api.DTO.Survey
{
    public class SurveyDto
    {
        public int id { get; set; }
        public int companyid { get; set; }
        public long custid { get; set; }
        public string owner { get; set; } = string.Empty;
        public string titleno { get; set; } = string.Empty;
        public string surveyno { get; set; } = string.Empty;
        public DateTime? surveydate { get; set; }
        public string propertyname { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public decimal landarea { get; set; }
        public string details { get; set; } = string.Empty;
        public decimal contractprice { get; set; }
        public decimal balance { get; set; }
        public int status { get; set; }
        public long createdbyid { get; set; }
        public long updatedbyid { get; set; }
        public Client? client { get; set; }
    }
}
