using ires_api.Data;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace ires_api.Services.Repository
{
    public class EmployeeRepository : IEmployeeService
    {
        private readonly DataContext _dataContext;

        public EmployeeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Employee Create(Employee employee)
        {
            employee.employeeid = 0;
            employee.datecreated = DateTime.Now;
            _dataContext.employees.Add(employee);
            _dataContext.SaveChanges();
            return employee;
        }

        public Employee GetEmployeeByEmail(string email)
        {
            return _dataContext.employees.Where(x => x.email == email).FirstOrDefault();
        }

        public Employee GetEmployeeById(long id)
        {
            return _dataContext.employees.Find(id);
        }

        public Employee GetEmployeeByUsername(string username)
        {
            return _dataContext.employees.Where(x => x.username == username).FirstOrDefault();
        }

        public ICollection<Employee> GetEmployees(long companyID, string search)
        {
            return _dataContext.employees.Where(x => x.companyid == companyID && (x.firstname.Contains(search) || x.lastname.Contains(search) || (x.designation ?? "").Contains(search)))
                .OrderBy(x=> x.lastname + x.firstname).ToList();
        }

        public List<UserPrivilege> GetUserPrivileges(long id)
        {
            return _dataContext.userPrivileges.Where(x => x.userid == id).ToList();
        }

        public Employee Login(string username, string userpass)
        {
            return _dataContext.employees.Include(x => x.company).Where(x => (x.username == username || x.email == username) && x.userpass == userpass
                && (x.isactive ?? true)).FirstOrDefault();
        }

        public Employee Update(EmployeeRequestDto requestDto)
        {
            Employee employee = GetEmployeeById(requestDto.employeeid);
            if(employee != null)
            {
                employee.firstname = requestDto.firstname;
                employee.lastname = requestDto.lastname;
                employee.middlename = requestDto.middlename;
                employee.mobileno = requestDto.mobileno;
                employee.gender = requestDto.gender;
                employee.designation = requestDto.designation;
                employee.email = requestDto.email;
                employee.username = requestDto.username;
                employee.isactive = requestDto.isactive;
                employee.isappsysadmin = requestDto.isappsysadmin;

                _dataContext.SaveChanges();
            }
            return employee;
        }
    }
}
