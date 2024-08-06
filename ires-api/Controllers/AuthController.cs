using AutoMapper;
using ires.AppService.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Employee;
using ires.Domain.DTO.User;
using ires.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        IConfiguration _configuration,
        IEmployeeService _employeeService,
        ICompanyService _companyService,
        IMapper _mapper,
        ILogService _logService,
        IMailService _mailService) : ControllerBase
    {

        [HttpGet("testconnection")]
        [AllowAnonymous]
        public IActionResult TestConnection()
        {
            var response = new ServerResponse<string>
            {
                Data = "api is ok"
            };
            //response.Data = _configuration.GetValue<string>("uiBaseURL");
            return Ok(response);
        }
        [HttpGet("testdbconnection")]
        [AllowAnonymous]
        public async Task<IActionResult> TestDBConnection()
        {
            var response = new ServerResponse<Object>();
            var companies = await _companyService.GetCompanies();
            var data = new { companyCount = companies.Count };
            response.Data = data;
            return Ok(response);
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> login(UserLoginRequestDto requestDto)
        {
            var response = new ServerResponse<UserLoginViewModel>();
            var employee = await _employeeService.LoginAsync(requestDto.username, ires.Domain.Utility.GetHash(requestDto.password));
            if (employee == null)
            {
                response.Success = false;
                response.Message = "Invalid login credentials";
                return BadRequest(response);
            }
            else if (employee != null && !(employee.company?.isverified ?? false))
            {
                response.Data = _mapper.Map<UserLoginViewModel>(employee);
                response.Success = false;
                response.errorCode = 1;
                response.Message = "Your account is unverified. Please check your email.";
                return BadRequest(response);
            }

            response.Data = _mapper.Map<UserLoginViewModel>(employee);
            //response.Data.userPrivileges = await _employeeService.GetUserPrivilegesByModule(employee.employeeid);
            //response.Data.LoadPrivileges(_mapper, await _employeeService.GetUserPrivileges(employee.employeeid));
            response.Data.Token = GenerateToken(employee);
            await _logService.SaveLogAsync(employee.companyid, employee.employeeid, AppModule.Users, "Authentication", "User logged in : " + response.Data.Token, 0);
            return Ok(response);
        }

        private string GenerateToken(EmployeeViewModel employee)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employee.username ?? ""),
                new Claim(ClaimTypes.PrimarySid, employee.employeeid.ToString()),
                new Claim(ClaimTypes.PrimaryGroupSid, employee.companyid.ToString()),
                new Claim(ClaimTypes.Email, employee.email ?? ""),
                new Claim(ClaimTypes.GivenName, employee.firstname ?? ""),
                new Claim(ClaimTypes.Surname, employee.lastname ?? ""),
                new Claim(ClaimTypes.Role, (employee.isappsysadmin) ? "Admin" : "User")
            };
            var token = new JwtSecurityToken(_configuration["Jwt.Issuer"],
                _configuration["Jwt.Audience"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("systemoverride")]
        public async Task<IActionResult> SystemOverride(UserLoginRequestDto requestDto)
        {
            var response = new ServerResponse<UserLoginViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                response.Success = false;
                response.Message = "Unable to process request";
                return BadRequest(response);
            }
            var employee = await _employeeService.LoginAsync(requestDto.username, ires.Domain.Utility.GetHash(requestDto.password));

            if (employee == null)
            {
                response.Success = false;
                response.Message = "Invalid login credentials";
                return BadRequest(response);
            }
            else if (employee != null && employee.companyid != (identity.companyid ?? 0))
            {
                response.Success = false;
                response.Message = "Invalid login credentials";
                return BadRequest(response);
            }
            else if (employee != null && !(employee.company?.isverified ?? false))
            {
                response.Success = false;
                response.Message = "Your account is unverified. Please check your email.";
                return BadRequest(response);
            }
            else if (employee != null && !(employee.isappsysadmin))
            {
                response.Success = false;
                response.Message = "Your account is not an Admin.";
                return BadRequest(response);
            }

            response.Data = _mapper.Map<UserLoginViewModel>(employee);
            return Ok(response);
        }

        [HttpPost("sendpasswordresetlink")]
        [AllowAnonymous]
        public async Task<IActionResult> SendPasswordResetLink(StringViewModel stringDto)
        {
            var response = new ServerResponse<UserLoginViewModel>();
            var employee = await _employeeService.GetEmployeeByEmail(stringDto.value);
            if (employee == null)
            {
                response.Success = false;
                response.Message = "Sorry, we couldn't find you email in our list";
                return BadRequest(response);
            }
            if (!(employee.isactive) || !(employee.company.isverified))
            {
                response.Success = false;
                response.Message = "Sorry, your account is not active or unverified";
                return BadRequest(response);
            }
            string token = await _employeeService.CreatePasswordResetToken(employee.employeeid);

            var html = System.IO.File.ReadAllText(@"./Templates/PasswordReset.html");
            var body = html.Replace("{1}", employee.firstname).Replace("{0}", _configuration["uiBaseURL"] + "/resetpassword?token=" + token);
            _mailService.SendEmailAsync("Reset your HexaByt Password", [stringDto.value], body, true);
            await _logService.SaveLogAsync(employee.companyid, employee.employeeid, AppModule.Users, "Profile", "Reset Password link request :" + token, 0);

            return Ok(response);
        }
        [HttpGet("validatepasswordresettoken")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidatePasswordResetToken(string token)
        {
            var response = new ServerResponse<UserLoginViewModel>();
            var employee = await _employeeService.GetPasswordToken(token);
            response.Success = employee != null;
            return Ok(response);
        }

        [HttpPut("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<EmployeeViewModel>();
            var employee = await _employeeService.GetPasswordToken(requestDto.token);
            if (employee == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Sorry, your password reset link is expired";
                return BadRequest(serverResponse);
            }
            await _employeeService.ChangePassword(employee.employeeid, ires.Domain.Utility.GetHash(requestDto.newuserpass), true);
            return Ok(serverResponse);
        }
    }
}
