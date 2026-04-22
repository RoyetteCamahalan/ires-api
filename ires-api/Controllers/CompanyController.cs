using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Company;
using ires.Domain.DTO.CompanySetting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController(
        IConfiguration configuration, 
        ICompanyService companyService, 
        IEmployeeService employeeService, 
        IAccountService accountService,
        IExpenseService expenseService,
        IMailService mailService) : ControllerBase
    {
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
            var company = await companyService.GetByID(identity.companyid ?? 0);
            serverResponse.Data = company;
            return Ok(serverResponse);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> register([FromBody] RegisterCompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<CompanyViewModel>();
            var company = await companyService.GetCompanyByName(requestDto.name);
            if (company != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Company is already registered.";
                return BadRequest(serverResponse);
            }
            if (await employeeService.GetEmployeeByEmail(requestDto.email) != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Email already in use.";
                return BadRequest(serverResponse);
            }
            company = await companyService.RegisterAsync(requestDto);
            this.sendConfirmationEmail(company.id, requestDto.email);
            serverResponse.Data = company;
            return Ok(serverResponse);
        }
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.id = identity.companyid ?? 0;
            requestDto.updatedbyid = identity.employeeid;
            if (!await companyService.Update(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Oops! We are unable to process this request.";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }

        private void sendConfirmationEmail(int id, string email)
        {
            var html = System.IO.File.ReadAllText(@"./Templates/ConfirmationEmail.html");
            var body = html.Replace("{0}", configuration["uiBaseURL"]).Replace("{1}", configuration["uiBaseURL"] + "/company/confirmation?ref=" + Utility.URLEncrypt(id.ToString()));
            mailService.SendEmailAsync("Email Confirmation", new List<string> { email }, body, true);
        }

        [HttpPost("resendconfirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmationAsync(IDRequestDto requestDto)
        {
            var employees = await employeeService.GetEmployees((int)requestDto.id, "");
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
                var verified = await companyService.Verify(id);
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
        [HttpPost("completetour/{id}")]
        public async Task<IActionResult> CompleteTour(int id)
        {
            var serverResponse = new ServerResponse<bool>();
            if (!await companyService.CompleteTour(id))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Oops! We are unable to process this request.";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }

        [HttpGet("settings")]
        public async Task<IActionResult> GetSettings()
        {
            var serverResponse = new ServerResponse<CompanySettingViewModel>();
            serverResponse.Data = await companyService.GetSettings();
            return Ok(serverResponse);
        }

        [HttpPost("settings")]
        public async Task<IActionResult> UpdateSettings(CompanySettingDto requestDto)
        {
            if (await companyService.UpdateSettings(requestDto))
            {
                return Ok(new ServerResponse<Boolean> { Data = true });
            }
            return BadRequest(new ServerResponse<Boolean> { Data = false });
        }

        [HttpGet("preload")]
        public async Task<IActionResult> DataPreload()
        {
            var offices = await accountService.GetOffices("", true);
            var expenseTypes = await expenseService.GetExpenseTypes(true, "");
            return Ok(new { 
                offices,
                expenseTypes
            });
        }
    }
}
