using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IEmployeeService
    {
        public ICollection<Employee> GetEmployees(long companyID, string search);
        public Employee GetEmployeeById(long id);
        public Employee GetEmployeeByEmail(string email);
        public Employee GetEmployeeByUsername(string username);
        public Employee Login(string username, string userpass);
        public List<UserPrivilege> GetUserPrivileges(long id);

        public Employee Create(Employee employee);
        public Employee Update(EmployeeRequestDto requestDto);
    }
}
