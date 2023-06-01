namespace ires_api.DTO
{
    public class EventDto
    {
        public long id { get; set; }
        public int moduleid { get; set; }
        public string title { get; set; } = string.Empty;
        public DateTime date { get; set; }

        public SurveyDto? survey { get; set; }
    }
}
