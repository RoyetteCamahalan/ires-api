using ires.AppService.Common;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.BankAccount;
using ires.Domain.DTO.Office;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService _accountService) : ControllerBase
    {

        [HttpGet("getbankaccounts")]
        public async Task<IActionResult> GetBankAccounts([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<BankAccountViewModel>>
            {
                Data = await _accountService.GetBankAccounts(request)
            };
            return Ok(serverResponse);

        }

        [HttpGet("getbankaccount/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<BankAccountViewModel>();
            var bankAccount = await _accountService.GetBankAccountByID(id);
            serverResponse.Data = bankAccount;
            return Ok(serverResponse);
        }

        [HttpPost("createbankaccount")]
        public async Task<IActionResult> Post([FromBody] BankAccountRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BankAccountViewModel>();
            if (await _accountService.isBankAccountExist(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.message = "Bank account already exist";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = await _accountService.CreateBankAccountAsync(requestDto);
            return Ok(serverResponse);
        }

        [HttpPut("updatebankaccount")]
        public async Task<IActionResult> Put([FromBody] BankAccountRequestDto requestDto)
        {
            await _accountService.UpdateBankAccountAsync(requestDto);
            return Ok(new ServerResponse<bool>());
        }





        [HttpGet("getoffice")]
        public async Task<IActionResult> GetOffice(long id)
        {
            var serverResponse = new ServerResponse<OfficeViewModel>();
            var result = await _accountService.GetOfficeByID(id);
            serverResponse.Data = result;
            return Ok(serverResponse);

        }

        [HttpGet("getoffices")]
        public async Task<IActionResult> GetOffices([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<OfficeViewModel>>
            {
                Data = await _accountService.GetOffices(request)
            };
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
                serverResponse.message = "Bank already registered.";
                return BadRequest(serverResponse);
            }
            result = await _accountService.CreateOfficeAsync(requestDto);
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut("updateoffice")]
        public async Task<IActionResult> UpdateOffice([FromBody] OfficeRequestDto requestDto)
        {
            await _accountService.UpdateOfficeAsync(requestDto);
            return Ok(new ServerResponse<bool>());
        }
    }
}
