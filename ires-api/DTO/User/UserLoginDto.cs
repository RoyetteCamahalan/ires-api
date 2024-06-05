using ires_api.DTO.Company;

namespace ires_api.DTO.User
{
    public class UserLoginDto
    {
        public long employeeid { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? middlename { get; set; }
        public bool? isappsysadmin { get; set; }
        public string Token { get; set; } = string.Empty;
        public int? companyid { get; set; }
        public CompanyDto? company { get; set; }
        public ICollection<UserAccessDto>? userPrivileges { get; set; }

        //public List<UserPrivilegeDto>? userPrivileges { get; set; }

        //public void LoadPrivileges(IMapper mapper, ICollection<UserPrivilege> up)
        //{
        //    userPrivileges = new List<UserPrivilegeDto>();
        //    foreach (var userPrivilege in up)
        //    {
        //        userPrivileges.Add(mapper.Map<UserPrivilegeDto>(userPrivilege));
        //    }
        //}
    }
}
