using ires_api.DTO.Survey;
using ires_api.Enumerations;

namespace ires_api.DTO
{
    public class EventViewModel
    {
        public long id { get; set; }
        public AppModule moduleid { get; set; }
        public string title { get; set; } = string.Empty;
        public DateTime date { get; set; }

        public SurveyViewModel? survey { get; set; }
    }
}
