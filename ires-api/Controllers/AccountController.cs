using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.BankAccount;
using ires.Domain.DTO.Office;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogService _logService;

        public AccountController(IAccountService accountService, ILogService logService)
        {
            _accountService = accountService;
            _logService = logService;
        }

        [HttpGet("getbankaccounts")]
        public async Task<IActionResult> GetBankAccounts(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<BankAccountViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var bankAccounts = await _accountService.GetBankAccounts(identity.companyid ?? 0, search ?? "");
            var paginator = new PaginatorDto<BankAccountViewModel>(currentPage);
            paginator.Paginate(bankAccounts);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("getbankaccount/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<BankAccountViewModel>();
            var bankAccount = await _accountService.GetBankAccountByID(id);
            if (bankAccount == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = bankAccount;
            return Ok(serverResponse);
        }

        [HttpPost("createbankaccount")]
        public async Task<IActionResult> Post([FromBody] BankAccountRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BankAccountViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (await _accountService.isBankAccountExist(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Bank account already exist";
                return BadRequest(serverResponse);
            }
            var result = await _accountService.CreateBankAccountAsync(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Create New Bank Account", "Account : " + result.accountid + '-' + requestDto.accountname, 0);
            return Ok(serverResponse);
        }

        [HttpPut("updatebankaccount")]
        public async Task<IActionResult> Put([FromBody] BankAccountRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (!await _accountService.UpdateBankAccountAsync(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            _logService.SaveLog(identity.companyid ?? 0, identity.employeeid, 0, "Updated Bank Account", "Account ID: " + requestDto.accountid + '-' + requestDto.accountname, 0);
            return Ok(serverResponse);
        }





        [HttpGet("getoffice")]
        public async Task<IActionResult> GetOffice(long id)
        {
            var serverResponse = new ServerResponse<OfficeViewModel>();
            var result = await _accountService.GetOfficeByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);

        }

        [HttpGet("getoffices")]
        public async Task<IActionResult> GetOffices(int currentPage, bool viewAll, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<OfficeViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _accountService.GetOffices(identity.companyid ?? 0, search ?? "", viewAll);
            var paginator = new PaginatorDto<OfficeViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost("createoffice")]
        public async Task<IActionResult> CreateOffice([FromBody] OfficeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<OfficeViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _accountService.GetOfficeByName(requestDto.companyid, requestDto.accountname);
            if (result != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Bank already registered.";
                return BadRequest(serverResponse);
            }
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            result = await _accountService.CreateOfficeAsync(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Office", "Create New Office : " + result.accountid + "-" + requestDto.accountname, 0);
            return Ok(serverResponse);
        }

        [HttpPut("updateoffice")]
        public async Task<IActionResult> UpdateOffice([FromBody] OfficeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<OfficeViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            if (!await _accountService.UpdateOfficeAsync(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            _logService.SaveLog(identity.companyid ?? 0, identity.employeeid, 0, "Office", "Update Office ID : " + requestDto.accountid, 0);
            return Ok(serverResponse);
        }
    }
}
