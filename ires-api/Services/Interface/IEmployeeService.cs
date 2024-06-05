using ires_api.DTO.Employee;
using ires_api.DTO.User;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IEmployeeService
    {
        public Task<ICollection<Employee>> GetEmployees(long companyID, string search);
        public Task<Employee> GetEmployeeById(long id);
        public Task<Employee> GetEmployeeByEmail(string email);
        public Task<Employee> GetEmployeeByUsername(string username);
        public Task<Employee> LoginAsync(string username, string userpass);
        public Task<string> CreatePasswordResetToken(long id);
        public Task<Employee> GetPasswordToken(string token);

        public Task<Employee> CreateAsync(Employee employee);
        public Task<Employee> UpdateAsync(EmployeeRequestDto requestDto);
        public Task ChangePassword(long id, string newPassword);

        public Task<ICollection<UserPrivilege>> GetUserPrivileges(long id);
        public Task<ICollection<UserPrivilege>> GetUserAllPrivileges(long id);
        public Task<ICollection<UserAccessDto>> GetUserPrivilegesByModule(long id);
        public Task<bool> CreateUserPrivileges(List<UserPrivilege> userPrivileges, long createdByID);
    }
}
