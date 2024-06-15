using ires.Domain.DTO;
using ires.Domain.DTO.Employee;
using ires.Domain.DTO.User;

namespace ires.Domain.Contracts
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

        public Task<ICollection<UserPrivilegeViewModel>> GetUserPrivileges(long id);
        public Task<ICollection<UserPrivilegeViewModel>> GetUserAllPrivileges(long id);
        public Task<ICollection<UserAccessViewModel>> GetUserPrivilegesByModule(long id);
        public Task<bool> CreateUserPrivileges(List<UserPrivilegeRequestDto> requestDtos, long createdByID);
    }
}
