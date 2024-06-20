using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICompanyService _companyService;
        private readonly IEmployeeService _employeeService;
        private readonly IMailService _mailService;

        public CompanyController(IConfiguration configuration, ICompanyService companyService, IEmployeeService employeeService, IMailService mailService)
        {
            _configuration = configuration;
            _companyService = companyService;
            _employeeService = employeeService;
            _mailService = mailService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var serverResponse = new ServerResponse<CompanyViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var company = await _companyService.GetByID(identity.companyid ?? 0);
            serverResponse.Data = company;
            return Ok(serverResponse);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> register([FromBody] CompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<CompanyViewModel>();
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
            this.sendConfirmationEmail(company.id, requestDto.email);
            serverResponse.Data = company;
            return Ok(serverResponse);
        }

        private void sendConfirmationEmail(int id, string email)
        {
            var html = System.IO.File.ReadAllText(@"./Templates/ConfirmationEmail.html");
            var body = html.Replace("{0}", _configuration["uiBaseURL"]).Replace("{1}", _configuration["uiBaseURL"] + "/company/confirmation?ref=" + Utility.URLEncrypt(id.ToString()));
            _mailService.SendEmailAsync("Email Confirmation", new List<string> { email }, body, true);
        }

        [HttpPost("resendconfirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmationAsync(IDRequestDto requestDto)
        {
            var employees = await _employeeService.GetEmployees((int)requestDto.id, "");
            if (employees.Count > 0)
            {
                sendConfirmationEmail((int)requestDto.id, employees.First().email);
            }
            return Ok(new ServerResponse<Boolean> { Data = true });
        }

        [HttpPost("verify")]
        [AllowAnonymous]
        public async Task<IActionResult> verify(StringViewModel slug)
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
