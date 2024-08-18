using ires.AppService.Common;
using ires.Domain;
using ires.Domain.Common;
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
    public class EmployeeController(
        IEmployeeService _employeeService,
        ICurrentUserService _currentUserService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<EmployeeViewModel>>
            {
                Data = await _employeeService.GetEmployees(request)
            };
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
                serverResponse.message = "Email already registered.";
                return BadRequest(serverResponse);
            }
            employee = await _employeeService.GetEmployeeByUsername(requestDto.username ?? "");
            if (employee != null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Username already in use";
                return BadRequest(serverResponse);
            }
            requestDto.userpass = Utility.GetHash(requestDto.userpass ?? "password");
            var result = await _employeeService.CreateAsync(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = employee;
            return Ok(serverResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] EmployeeRequestDto requestDto)
        {
            await _employeeService.UpdateAsync(requestDto);
            return Ok(new ServerResponse<bool>());
        }

        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var result = await _employeeService.ChangePassword(_currentUserService.employeeid, Utility.GetHash(requestDto.newuserpass));
            if (result.value != "")
            {
                serverResponse.Success = false;
                serverResponse.message = result.value;
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
