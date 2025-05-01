namespace ires.Core.ViewModels
{
    public class AgentViewModel
    {
        public long id { get; set; }
        public Guid guid { get; set; }
        public string firstname { get; set; } = string.Empty;
        public string lastname { get; set; } = string.Empty;
        public string fullname { get => firstname + " " + lastname; }
        public string contactno { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string tinnumber { get; set; } = string.Empty;
        public bool isactive { get; set; } = true;
        public long upline_id { get; set; }
        public AgentViewModel? upline { get; set; }
    }
}
