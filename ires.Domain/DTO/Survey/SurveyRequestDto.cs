using ires.Domain.Enumerations;

namespace ires.Domain.DTO.Survey
{
    public class SurveyRequestDto
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
        public SurveyStatus status { get; set; }
        public long createdbyid { get; set; }
        public long updatedbyid { get; set; }
    }
}
