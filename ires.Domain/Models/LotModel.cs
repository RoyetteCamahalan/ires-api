namespace ires.Domain.Models
{
    public class LotModel
    {
        public long id { get; set; }
        public long project_id { get; set; }
        public string name { get; set; } = string.Empty;
    }
}
