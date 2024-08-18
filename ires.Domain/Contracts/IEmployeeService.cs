using ires.Domain.Common;
using ires.Domain.DTO;
using ires.Domain.DTO.Employee;
using ires.Domain.DTO.User;

namespace ires.Domain.Contracts
{
    public interface IEmployeeService
    {
        public Task<ICollection<EmployeeViewModel>> GetEmployees(long companyID);
        public Task<PaginatedResult<EmployeeViewModel>> GetEmployees(PaginationRequest paginationRequest);
        public Task<EmployeeViewModel> GetByID(long id);
        public Task<EmployeeViewModel> GetEmployeeByEmail(string email);
        public Task<EmployeeViewModel> GetEmployeeByUsername(string username);
        public Task<EmployeeViewModel> LoginAsync(string username, string userpass);
        public Task<string> CreatePasswordResetToken(long id);
        public Task<EmployeeViewModel> GetPasswordToken(string token);

        public Task<EmployeeViewModel> CreateAsync(EmployeeRequestDto requestDto);
        public Task UpdateAsync(EmployeeRequestDto requestDto);
        public Task<StringViewModel> ChangePassword(long id, string newPassword, bool withToken = false);

        public Task<ICollection<UserPrivilegeViewModel>> GetUserPrivileges(long id);
        public Task<ICollection<UserPrivilegeViewModel>> GetUserAllPrivileges(long id);
        public Task<ICollection<UserAccessViewModel>> GetUserPrivilegesByModule(long id);
        public Task<bool> CreateUserPrivileges(List<UserPrivilegeRequestDto> requestDtos, long createdByID);
    }
}
