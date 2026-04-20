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

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("getbankaccounts")]
        public async Task<IActionResult> GetBankAccounts(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<BankAccountViewModel>>();
            var bankAccounts = await _accountService.GetBankAccounts(search ?? "");
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
            requestDto.createdbyid = identity.employeeid;
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
            return Ok(serverResponse);
        }

        [HttpPut("updatebankaccount")]
        public async Task<IActionResult> Put([FromBody] BankAccountRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            if (!await _accountService.UpdateBankAccountAsync(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
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
            var result = await _accountService.GetOffices(search ?? "", viewAll);
            var paginator = new PaginatorDto<OfficeViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost("createoffice")]
        public async Task<IActionResult> CreateOffice([FromBody] OfficeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<OfficeViewModel>();
            var result = await _accountService.GetOfficeByName(requestDto.accountname);
            if (result != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Bank already registered.";
                return BadRequest(serverResponse);
            }
            result = await _accountService.CreateOfficeAsync(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut("updateoffice")]
        public async Task<IActionResult> UpdateOffice([FromBody] OfficeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<OfficeViewModel>();
            if (!await _accountService.UpdateOfficeAsync(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
    }
}
