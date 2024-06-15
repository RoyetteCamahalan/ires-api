using ires.Domain.DTO.Company;

namespace ires.Domain.DTO.User
{
    public class UserLoginViewModel
    {
        public long employeeid { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? middlename { get; set; }
        public bool isappsysadmin { get; set; }
        public string Token { get; set; } = string.Empty;
        public int? companyid { get; set; }
        public CompanyViewModel? company { get; set; }
        public ICollection<UserAccessViewModel>? userPrivileges { get; set; }

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
