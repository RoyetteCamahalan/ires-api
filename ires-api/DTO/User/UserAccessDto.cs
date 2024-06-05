namespace ires_api.DTO.User
{
    public class UserAccessDto
    {

        public int moduleid { get; set; }
        public string modulename { get; set; } = string.Empty;
        public int moduletypeid { get; set; }
        public bool isactive { get; set; }
        public UserPrivilegeDto? access { get; set; }
        //public UserPrivilegeDto? survey { get; set; }
        //public UserPrivilegeDto? payment { get; set; }
        //public UserPrivilegeDto? client { get; set; }
        //public UserPrivilegeDto? user { get; set; }
        //public UserPrivilegeDto? banks { get; set; }
        //public UserPrivilegeDto? bankaccounts { get; set; }
        //public UserPrivilegeDto? offices { get; set; }
        //public UserPrivilegeDto? vendors { get; set; }
        //public UserPrivilegeDto? expensetypes { get; set; }
        //public UserPrivilegeDto? expense { get; set; }
        //public UserPrivilegeDto? pettycash { get; set; }
        //public UserPrivilegeDto? accountspayable { get; set; }
    }
}
