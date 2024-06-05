using ires_api.Data;
using ires_api.DTO;
using ires_api.DTO.Employee;
using ires_api.DTO.User;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
{
    public class EmployeeRepository : IEmployeeService
    {
        private readonly DataContext _dataContext;

        public EmployeeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            employee.employeeid = 0;
            employee.datecreated = DateTime.Now;
            _dataContext.employees.Add(employee);
            await _dataContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            return await _dataContext.employees.Include(x => x.company).Where(x => x.email == email).FirstOrDefaultAsync();
        }

        public async Task<Employee> GetEmployeeById(long id)
        {
            return await _dataContext.employees.FindAsync(id);
        }

        public async Task<Employee> GetEmployeeByUsername(string username)
        {
            return await _dataContext.employees.Where(x => x.username == username).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Employee>> GetEmployees(long companyID, string search)
        {
            return await _dataContext.employees.Where(x => x.companyid == companyID && (x.firstname.Contains(search) || x.lastname.Contains(search) || (x.designation ?? "").Contains(search)))
                .OrderBy(x => x.lastname + x.firstname).ToListAsync();
        }

        public async Task<Employee> LoginAsync(string username, string userpass)
        {
            return await _dataContext.employees.Include(x => x.company).Where(x => (x.username == username || x.email == username) && x.userpass == userpass
                && (x.isactive ?? true)).FirstOrDefaultAsync();
        }

        public async Task<Employee> UpdateAsync(EmployeeRequestDto requestDto)
        {
            Employee employee = await GetEmployeeById(requestDto.employeeid);
            if (employee != null)
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

                await _dataContext.SaveChangesAsync();
            }
            return employee;
        }

        public async Task ChangePassword(long id, string newPassword)
        {
            var data = await GetEmployeeById(id);
            data.userpass = newPassword;
            data.passwordresettoken = "";
            await _dataContext.SaveChangesAsync();
        }

        public async Task<string> CreatePasswordResetToken(long id)
        {
            var employee = await GetEmployeeById(id);
            var token = Utility.RandomString(16);
            while (await _dataContext.employees.AnyAsync(x => x.passwordresettoken == token))
            {
                token = Utility.RandomString(16);
            }
            employee.passwordresettoken = token;
            await _dataContext.SaveChangesAsync();
            return token;
        }

        public async Task<Employee> GetPasswordToken(string token)
        {
            return await _dataContext.employees.Where(x => x.passwordresettoken == token).FirstOrDefaultAsync();
        }

        public async Task<UserPrivilege> GetUserPrivilegeByID(long id)
        {
            return await _dataContext.userPrivileges.FindAsync(id);
        }

        public async Task<ICollection<UserPrivilege>> GetUserPrivileges(long id)
        {
            return await _dataContext.userPrivileges.Where(x => x.userid == id).ToListAsync();
        }

        public async Task<ICollection<UserPrivilege>> GetUserAllPrivileges(long id)
        {
            return await (from pm in _dataContext.planModules
                          join up in _dataContext.userPrivileges.Where(x => x.userid == id) on pm.moduleid equals up.moduleid into ups
                          from joinups in ups.DefaultIfEmpty()
                          select new UserPrivilege
                          {
                              userprivid = joinups == null ? 0 : joinups.userid,
                              moduleid = joinups == null ? pm.moduleid : joinups.moduleid,
                              canadd = joinups == null ? false : joinups.canadd,
                              canedit = joinups == null ? false : joinups.canedit,
                              canview = joinups == null ? false : joinups.canview,
                              canaccess = joinups == null ? false : joinups.canaccess,
                              canverify = joinups == null ? false : joinups.canverify,
                              canvoid = joinups == null ? false : joinups.canvoid,
                              module = pm.module
                          }).ToListAsync();
        }
        public async Task<ICollection<UserAccessDto>> GetUserPrivilegesByModule(long id)
        {
            return await (from pm in _dataContext.planModules
                          join up in _dataContext.userPrivileges.Where(x => x.userid == id) on pm.moduleid equals up.moduleid into ups
                          from joinups in ups.DefaultIfEmpty()
                          select new UserAccessDto
                          {
                              moduleid = joinups == null ? pm.moduleid : joinups.moduleid,
                              access = new UserPrivilegeDto
                              {
                                  userprivid = joinups == null ? 0 : joinups.userid,
                                  canadd = joinups == null ? false : joinups.canadd,
                                  canedit = joinups == null ? false : joinups.canedit,
                                  canview = joinups == null ? false : joinups.canview,
                                  canaccess = joinups == null ? false : joinups.canaccess,
                                  canverify = joinups == null ? false : joinups.canverify,
                                  canvoid = joinups == null ? false : joinups.canvoid,
                              },
                              modulename = pm.module.modulename
                          }).ToListAsync();
        }

        public async Task<bool> CreateUserPrivileges(List<UserPrivilege> userPrivileges, long createdByID)
        {
            foreach (var userPrivilege in userPrivileges)
            {
                if (userPrivilege.userprivid == 0)
                {
                    userPrivilege.datecreated = DateTime.Now;
                    userPrivilege.createdbyid = createdByID;
                    await _dataContext.userPrivileges.AddAsync(userPrivilege);
                }
                else
                {
                    var data = await GetUserPrivilegeByID(userPrivilege.userprivid);
                    if (data != null)
                    {
                        data.canadd = userPrivilege.canadd;
                        data.canedit = userPrivilege.canedit;
                        data.canview = userPrivilege.canview;
                        data.canverify = userPrivilege.canverify;
                        data.canvoid = userPrivilege.canvoid;
                        data.canaccess = data.canadd || data.canedit || data.canview;
                    }
                }
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
