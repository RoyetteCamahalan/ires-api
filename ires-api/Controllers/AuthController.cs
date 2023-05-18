using AutoMapper;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
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
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public AuthController(IConfiguration configuration, IEmployeeService employeeService, IMapper mapper, ILogService logService)
        {
            _configuration = configuration;
            _employeeService = employeeService;
            _mapper = mapper;
            _logService = logService;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> login(UserLoginRequestDto requestDto)
        {
            var response = new ServerResponse<UserLoginDto>();
            var employee = await _employeeService.LoginAsync(requestDto.username, Utility.GetHash(requestDto.password));
            if (employee == null)
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

            response.Data = _mapper.Map<UserLoginDto>(employee);
            response.Data.userPrivileges = await _employeeService.GetUserPrivilegesByModule(employee.employeeid);
            //response.Data.LoadPrivileges(_mapper, await _employeeService.GetUserPrivileges(employee.employeeid));
            response.Data.Token = GenerateToken(employee);
            _logService.SaveLog(employee.companyid, employee.employeeid, 0, "Authentication", "User logged in : " + response.Data.Token, 0);
            return Ok(response);
        }

        private string GenerateToken(Employee employee)
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
                new Claim(ClaimTypes.Role, (employee.isappsysadmin ?? false) ? "Admin" : "User")
            };
            var token = new JwtSecurityToken(_configuration["Jwt.Issuer"],
                _configuration["Jwt.Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("systemoverride")]
        public async Task<IActionResult> SystemOverride(UserLoginRequestDto requestDto)
        {
            var response = new ServerResponse<UserLoginDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                response.Success = false;
                response.Message = "Unable to process request";
                return BadRequest(response);
            }
            var employee = await _employeeService.LoginAsync(requestDto.username, Utility.GetHash(requestDto.password));

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
            else if (employee != null && !(employee.isappsysadmin ?? false))
            {
                response.Success = false;
                response.Message = "Your account is not an Admin.";
                return BadRequest(response);
            }

            response.Data = _mapper.Map<UserLoginDto>(employee);
            return Ok(response);
        }
    }
}
