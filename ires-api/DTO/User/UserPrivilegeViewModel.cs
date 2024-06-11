namespace ires_api.DTO.User
{
    public class UserPrivilegeViewModel
    {
        public long userprivid { get; set; }
        public int moduleid { get; set; }
        public long userid { get; set; }
        public bool canadd { get; set; } = false;
        public bool canedit { get; set; } = false;
        public bool canview { get; set; } = false;
        public bool canverify { get; set; } = false;
        public bool canaccess { get; set; } = false;
        public bool canvoid { get; set; } = false;
        public ModuleDto? module { get; set; }
    }
}
