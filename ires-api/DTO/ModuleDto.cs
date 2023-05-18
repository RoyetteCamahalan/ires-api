namespace ires_api.DTO
{
    public class ModuleDto
    {
        public int moduleid { get; set; }
        public string modulename { get; set; } = string.Empty;
        public int moduletypeid { get; set; }
        public bool isactive { get; set; }
    }
}
