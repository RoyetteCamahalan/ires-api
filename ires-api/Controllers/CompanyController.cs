using ires.AppService.Common;
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
    public class CompanyController(
        IConfiguration _configuration,
        ICompanyService _companyService,
        IEmployeeService _employeeService,
        IMailService _mailService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var serverResponse = new ServerResponse<CompanyViewModel>
            {
                Data = await _companyService.GetByID()
            };
            return Ok(serverResponse);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> register([FromBody] RegisterCompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<CompanyViewModel>();
            var company = await _companyService.GetCompanyByName(requestDto.name);
            if (company != null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Company is already registered.";
                return BadRequest(serverResponse);
            }
            if (await _employeeService.GetEmployeeByEmail(requestDto.email) != null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Email already in use.";
                return BadRequest(serverResponse);
            }
            company = await _companyService.RegisterAsync(requestDto);
            this.sendConfirmationEmail(company.id, requestDto.email);
            serverResponse.Data = company;
            return Ok(serverResponse);
        }
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyRequestDto requestDto)
        {
            await _companyService.Update(requestDto);
            return Ok(new ServerResponse<bool>());
        }

        private void sendConfirmationEmail(int id, string email)
        {
            var html = System.IO.File.ReadAllText(@"./Templates/ConfirmationEmail.html");
            var body = html.Replace("{0}", _configuration["uiBaseURL"]).Replace("{1}", _configuration["uiBaseURL"] + "/company/confirmation?ref=" + Utility.URLEncrypt(id.ToString()));
            _mailService.SendEmailAsync("Email Confirmation", [email], body, true);
        }

        [HttpPost("resendconfirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmationAsync(IDRequestDto requestDto)
        {
            var employees = await _employeeService.GetEmployees((int)requestDto.id);
            if (employees.Count > 0)
            {
                sendConfirmationEmail((int)requestDto.id, employees.First().email);
            }
            return Ok(new ServerResponse<bool>());
        }

        [HttpPost("verify")]
        [AllowAnonymous]
        public async Task<IActionResult> verify(StringViewModel slug)
        {
            var serverResponse = new ServerResponse<string>();
            try
            {
                int id = Convert.ToInt32(Utility.URLDecrypt(slug.value));
                await _companyService.Verify(id);
                return Ok(serverResponse);
            }
            catch (Exception)
            {
                serverResponse.Success = false;
                serverResponse.message = "Oops! We are unable to process this request.";
                return BadRequest(serverResponse);
            }
        }
        [HttpPost("completetour/{id}")]
        public async Task<IActionResult> CompleteTour(int id)
        {
            await _companyService.CompleteTour(id);
            return Ok(new ServerResponse<bool>());
        }
    }
}
