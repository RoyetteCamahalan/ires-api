using ires.Domain.DTO.Survey;
using ires.Domain.Enumerations;

namespace ires.Domain.DTO
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
