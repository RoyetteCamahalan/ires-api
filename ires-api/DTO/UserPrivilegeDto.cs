namespace ires_api.DTO
{
    public class UserPrivilegeDto
    {
        public long userprivid { get; set; }
        public int moduleid { get; set; }
        public long userid { get; set; }
        public bool? canaccess { get; set; }
    }
}
