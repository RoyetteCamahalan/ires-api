using AutoMapper;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IMapper mapper, IEmployeeService employeeService) 
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult Get(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<EmployeeDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if(identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            List<EmployeeDto> employeeDtos = _mapper.Map <List<EmployeeDto>>(_employeeService.GetEmployees(identity.companyid ?? 0, search ?? ""));
            var paginator = new PaginatorDto<EmployeeDto>(currentPage);
            paginator.Paginate(employeeDtos);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(long id)
        {
            var serverResponse = new ServerResponse<EmployeeDto>();
            var employee = _employeeService.GetEmployeeById(id);
            if(employee == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<EmployeeDto>(employee);
            return Ok(serverResponse);
        }

        [HttpPost]
        public IActionResult Post([FromBody] EmployeeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<EmployeeDto>();
            var employee = _employeeService.GetEmployeeByEmail(requestDto.email ?? "");
            if(employee != null) {
                serverResponse.Success = false;
                serverResponse.Message = "Email already registered.";
                return BadRequest(serverResponse);
            }
            employee = _employeeService.GetEmployeeByUsername(requestDto.username ?? "");
            if (employee != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Username already in use";
                return BadRequest(serverResponse);
            }
            requestDto.userpass = Utility.GetHash(requestDto.userpass ?? "password");
            var result = _employeeService.Create(_mapper.Map<Employee>(requestDto));
            if(result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<EmployeeDto>(employee);
            return Ok(serverResponse);
        }

        [HttpPut]
        public IActionResult Put([FromBody] EmployeeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<EmployeeDto>();
            var employee = _employeeService.Update(requestDto);
            if (employee == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
    }
}
