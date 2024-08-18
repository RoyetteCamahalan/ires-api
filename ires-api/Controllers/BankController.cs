using ires.AppService.Common;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.Bank;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController(IBankService _bankService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get(long bankID)
        {
            var serverResponse = new ServerResponse<BankViewModel>
            {
                Data = await _bankService.GetBankByID(bankID)
            };
            return Ok(serverResponse);

        }

        [HttpGet("getallbanks")]
        public async Task<IActionResult> GetAllBanks([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<BankViewModel>>
            {
                Data = await _bankService.GetAllBanks(request)
            };
            return Ok(serverResponse);

        }
        [HttpGet("getbanks")]
        public async Task<IActionResult> GetBanks([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<BankViewModel>>()
            {
                Data = await _bankService.GetBanks(request)
            };
            return Ok(serverResponse);

        }
        [HttpGet("getewallets")]
        public async Task<IActionResult> GetEwallets([FromQuery] PaginationRequest request)
        {
            request.isEWallet = true;
            var serverResponse = new ServerResponse<PaginatedResult<BankViewModel>>()
            {
                Data = await _bankService.GetBanks(request)
            };
            return Ok(serverResponse);

        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BankRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BankViewModel>();
            var bank = await _bankService.GetBankByName(requestDto.name);
            if (bank != null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Bank already registered.";
                return BadRequest(serverResponse);
            }
            var result = await _bankService.Create(requestDto);
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] BankRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            await _bankService.Update(requestDto);
            return Ok(serverResponse);
        }
    }
}
