using AutoMapper;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;
        private readonly IEmployeeService _employeeService;
        private readonly IMailService _mailService;

        public CompanyController(IConfiguration configuration, IMapper mapper, ICompanyService companyService, IEmployeeService employeeService, IMailService mailService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _companyService = companyService;
            _employeeService = employeeService;
            _mailService = mailService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var serverResponse = new ServerResponse<CompanyDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var company = _companyService.GetCompanyByID(identity.companyid ?? 0);
            serverResponse.Data = _mapper.Map<CompanyDto>(company);
            return Ok(company);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> register([FromBody] CompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<CompanyDto>();
            var company = await _companyService.GetCompanyByName(requestDto.name);
            if (company != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Company is already registered.";
                return BadRequest(serverResponse);
            }
            if (await _employeeService.GetEmployeeByEmail(requestDto.email) != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Email already in use.";
                return BadRequest(serverResponse);
            }
            company = await _companyService.RegisterAsync(requestDto);
            requestDto.id = company.id;
            this.sendConfirmationEmail(requestDto);
            serverResponse.Data = _mapper.Map<CompanyDto>(company);
            return Ok(serverResponse);
        }

        private void sendConfirmationEmail(CompanyRequestDto requestDto)
        {
            var html = System.IO.File.ReadAllText(@"./Templates/ConfirmationEmail.html");
            var body = html.Replace("{0}", _configuration["uiBaseURL"]).Replace("{1}", _configuration["uiBaseURL"] + "/company/confirmation?ref=" + Utility.URLEncrypt(requestDto.id.ToString()));
            _mailService.SendEmailAsync("Email Confirmation", new List<string> { requestDto.email }, body, true);
        }


        [HttpPost("verify")]
        [AllowAnonymous]
        public async Task<IActionResult> verify(StringDto slug)
        {
            var serverResponse = new ServerResponse<string>();
            try
            {
                int id = Convert.ToInt32(Utility.URLDecrypt(slug.value));
                var verified = await _companyService.Verify(id);
                if (!verified)
                    throw new Exception("null");

                return Ok(serverResponse);
            }
            catch (Exception)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Oops! We are unable to process this request.";
                return BadRequest(serverResponse);
            }
        }
    }
}
