using AutoMapper;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public AuthController(IConfiguration configuration, IEmployeeService employeeService, IMapper mapper)
        {
            _configuration = configuration;
            _employeeService = employeeService;
            _mapper = mapper;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult login(UserLoginRequestDto requestDto)
        {
            var response = new ServerResponse<UserLoginDto>();
            var employee = _employeeService.Login(requestDto.username, Utility.GetHash(requestDto.password));
            if (employee == null)
            {
                response.Success = false;
                response.Message = "Invalid login credentials";
                return BadRequest(response);
            }
            else if(employee != null && !(employee.company?.isverified ?? false)){
                response.Success = false;
                response.Message = "Your account is unverified. Please check your email.";
                return BadRequest(response);
            }

            response.Data = _mapper.Map<UserLoginDto>(employee);
            response.Data.LoadPrivileges(_mapper, _employeeService.GetUserPrivileges(employee.employeeid));
            response.Data.Token = GenerateToken(employee);
            return Ok(response);
        }

        private string GenerateToken(Employee employee)
        {
            var test =_configuration["Jwt:Key"];
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
    }
}
