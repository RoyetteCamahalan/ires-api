using ires_api.DTO;
using ires_api.DTO.Employee;
using ires_api.DTO.User;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IEmployeeService
    {
        public Task<ICollection<EmployeeViewModel>> GetEmployees(long companyID, string search);
        public Task<EmployeeViewModel> GetByID(long id);
        public Task<EmployeeViewModel> GetEmployeeByEmail(string email);
        public Task<EmployeeViewModel> GetEmployeeByUsername(string username);
        public Task<EmployeeViewModel> LoginAsync(string username, string userpass);
        public Task<string> CreatePasswordResetToken(long id);
        public Task<EmployeeViewModel> GetPasswordToken(string token);

        public Task<EmployeeViewModel> CreateAsync(EmployeeRequestDto requestDto);
        public Task<bool> UpdateAsync(EmployeeRequestDto requestDto);
        public Task<StringViewModel> ChangePassword(long id, string newPassword);

        public Task<ICollection<UserPrivilege>> GetUserPrivileges(long id);
        public Task<ICollection<UserPrivilege>> GetUserAllPrivileges(long id);
        public Task<ICollection<UserAccessDto>> GetUserPrivilegesByModule(long id);
        public Task<bool> CreateUserPrivileges(List<UserPrivilege> userPrivileges, long createdByID);
    }
}
