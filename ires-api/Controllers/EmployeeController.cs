using ires.AppService.Common;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Employee;
using ires.Domain.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService _employeeService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<EmployeeViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _employeeService.GetEmployees(identity.companyid ?? 0, search ?? "");
            var paginator = new PaginatorDto<EmployeeViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<EmployeeViewModel>();
            var employee = await _employeeService.GetByID(id);
            if (employee == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = employee;
            return Ok(serverResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmployeeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<EmployeeViewModel>();
            var employee = await _employeeService.GetEmployeeByEmail(requestDto.email ?? "");
            if (employee != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Email already registered.";
                return BadRequest(serverResponse);
            }
            employee = await _employeeService.GetEmployeeByUsername(requestDto.username ?? "");
            if (employee != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Username already in use";
                return BadRequest(serverResponse);
            }
            requestDto.userpass = Utility.GetHash(requestDto.userpass ?? "password");
            var result = await _employeeService.CreateAsync(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = employee;
            return Ok(serverResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] EmployeeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<EmployeeViewModel>();
            if (!await _employeeService.UpdateAsync(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }

        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _employeeService.ChangePassword(identity.employeeid, Utility.GetHash(requestDto.newuserpass));
            if (result.value != "")
            {
                serverResponse.Success = false;
                serverResponse.Message = result.value;
                serverResponse.errorCode = 1;
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }

        [HttpGet("getpriviligesbymodule")]
        public async Task<IActionResult> GetPriviligesByModule(long id)
        {
            var serverResponse = new ServerResponse<ICollection<UserAccessViewModel>>();
            var result = await _employeeService.GetUserPrivilegesByModule(id);
            serverResponse.Data = result;
            return Ok(serverResponse);
        }
    }
}
