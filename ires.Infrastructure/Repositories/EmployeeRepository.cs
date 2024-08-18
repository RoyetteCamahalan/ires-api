using AutoMapper;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Employee;
using ires.Domain.DTO.User;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Infrastructure.Extensions;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class EmployeeRepository(DataContext _dataContext,
        IMapper _mapper,
        ILogService _logService,
        ICurrentUserService _currentUserService) : IEmployeeService
    {

        public async Task<EmployeeViewModel> CreateAsync(EmployeeRequestDto requestDto)
        {
            var entity = _mapper.Map<Employee>(requestDto);
            entity.employeeid = 0;
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.employees.Add(entity);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<EmployeeViewModel>(entity);
        }

        public async Task<EmployeeViewModel> GetEmployeeByEmail(string email)
        {
            var result = await _dataContext.employees.Include(x => x.company).Where(x => x.email == email).FirstOrDefaultAsync();
            return _mapper.Map<EmployeeViewModel>(result);
        }

        private async Task<Employee> GetEmployeeByID(long id)
        {
            return await _dataContext.employees.FindAsync(id) ?? throw new EntityNotFoundException();
        }

        public async Task<EmployeeViewModel> GetByID(long id)
        {
            var result = await GetEmployeeByID(id);
            return _mapper.Map<EmployeeViewModel>(result);
        }

        public async Task<EmployeeViewModel> GetEmployeeByUsername(string username)
        {
            var result = await _dataContext.employees.Where(x => x.username == username).FirstOrDefaultAsync();
            return _mapper.Map<EmployeeViewModel>(result);
        }

        public async Task<PaginatedResult<EmployeeViewModel>> GetEmployees(PaginationRequest request)
        {
            var query = _dataContext.employees.Where(x => x.companyid == _currentUserService.companyid
                && (x.firstname.Contains(request.searchString) || x.lastname.Contains(request.searchString) || (x.designation ?? "").Contains(request.searchString)))
                .OrderBy(x => x.lastname + x.firstname).AsQueryable();
            return await query.AsPaginatedResult<Employee, EmployeeViewModel>(request, _mapper.ConfigurationProvider);
        }
        public async Task<ICollection<EmployeeViewModel>> GetEmployees(long companyID)
        {
            var result = await _dataContext.employees.Where(x => x.companyid == companyID).OrderBy(x => x.lastname + x.firstname).ToListAsync();
            return _mapper.Map<ICollection<EmployeeViewModel>>(result);
        }

        public async Task<EmployeeViewModel> LoginAsync(string username, string userpass)
        {
            var result = await _dataContext.employees.Include(x => x.company).Where(x => (x.username == username || x.email == username) && x.userpass == userpass
            && x.isactive).FirstOrDefaultAsync();
            return _mapper.Map<EmployeeViewModel>(result);
        }

        public async Task UpdateAsync(EmployeeRequestDto requestDto)
        {
            var employee = await GetEmployeeByID(requestDto.employeeid);
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

        public async Task<StringViewModel> ChangePassword(long id, string newPassword, bool withToken = false)
        {
            var entity = await GetEmployeeByID(id);
            if (entity != null)
            {
                if (entity.userpass != newPassword)
                    return new StringViewModel("Old Password is incorrect");
                else if (entity.userpass == newPassword)
                    return new StringViewModel("New password should not be the same as your old password");

                entity.userpass = newPassword;
                entity.passwordresettoken = "";
                await _dataContext.SaveChangesAsync();
                if (withToken)
                    await _logService.SaveLogAsync(entity.companyid, id, AppModule.Users, "Profile", "Password Reset via token", 0);
                else
                    await _logService.SaveLogAsync(entity.companyid, id, AppModule.Users, "Profile", "Password Changed", 0);
                return new StringViewModel("");
            }
            return new StringViewModel("User not found");
        }

        public async Task<string> CreatePasswordResetToken(long id)
        {
            var employee = await GetEmployeeByID(id);
            var token = Utility.RandomString(16);
            while (await _dataContext.employees.AnyAsync(x => x.passwordresettoken == token))
            {
                token = Utility.RandomString(16);
            }
            employee.passwordresettoken = token;
            await _dataContext.SaveChangesAsync();
            return token;
        }

        public async Task<EmployeeViewModel> GetPasswordToken(string token)
        {
            var result = await _dataContext.employees.Where(x => x.passwordresettoken == token).FirstOrDefaultAsync();
            return _mapper.Map<EmployeeViewModel>(result);
        }

        public async Task<UserPrivilege> GetUserPrivilegeByID(long id)
        {
            return await _dataContext.userPrivileges.FindAsync(id);
        }

        public async Task<ICollection<UserPrivilegeViewModel>> GetUserPrivileges(long id)
        {
            var result = await _dataContext.userPrivileges.Where(x => x.userid == id).ToListAsync();
            return _mapper.Map<ICollection<UserPrivilegeViewModel>>(result);
        }

        public async Task<ICollection<UserPrivilegeViewModel>> GetUserAllPrivileges(long id)
        {
            var result = await (from pm in _dataContext.planModules
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
            return _mapper.Map<ICollection<UserPrivilegeViewModel>>(result);
        }
        public async Task<ICollection<UserAccessViewModel>> GetUserPrivilegesByModule(long id)
        {
            return await (from pm in _dataContext.planModules
                          join up in _dataContext.userPrivileges.Where(x => x.userid == id) on pm.moduleid equals up.moduleid into ups
                          from joinups in ups.DefaultIfEmpty()
                          select new UserAccessViewModel
                          {
                              moduleid = joinups == null ? pm.moduleid : joinups.moduleid,
                              access = new UserPrivilegeViewModel
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

        public async Task<bool> CreateUserPrivileges(List<UserPrivilegeRequestDto> requestDtos, long createdByID)
        {
            var userPrivileges = _mapper.Map<List<UserPrivilege>>(requestDtos);
            foreach (var userPrivilege in userPrivileges)
            {
                if (userPrivilege.userprivid == 0)
                {
                    userPrivilege.datecreated = Utility.GetServerTime();
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
