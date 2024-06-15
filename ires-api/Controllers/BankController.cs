using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Bank;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly ILogService _logService;

        public BankController(IBankService bankService, ILogService logService)
        {
            _bankService = bankService;
            _logService = logService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(long bankID)
        {
            var serverResponse = new ServerResponse<BankViewModel>();
            var banks = await _bankService.GetBankByID(bankID);
            if (banks == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = banks;
            return Ok(serverResponse);

        }

        [HttpGet("getallbanks")]
        public async Task<IActionResult> GetAllBanks(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<BankViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var banks = await _bankService.GetAllBanks(identity.companyid ?? 0, search ?? "");
            var paginator = new PaginatorDto<BankViewModel>(currentPage);
            paginator.Paginate(banks);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpGet("getbanks")]
        public async Task<IActionResult> GetBanks(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<BankViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var banks = await _bankService.GetBanks(identity.companyid ?? 0, false, search ?? "");
            var paginator = new PaginatorDto<BankViewModel>(currentPage);
            paginator.Paginate(banks);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpGet("getewallets")]
        public async Task<IActionResult> GetEwallets(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<BankViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var banks = await _bankService.GetBanks(identity.companyid ?? 0, true, search ?? "");
            var paginator = new PaginatorDto<BankViewModel>(currentPage);
            paginator.Paginate(banks);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BankRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BankViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var bank = await _bankService.GetBankByName(requestDto.companyid, requestDto.name);
            if (bank != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Bank already registered.";
                return BadRequest(serverResponse);
            }
            var result = await _bankService.Create(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Create New Bank", "Bank Name : " + requestDto.name, 0);
            return Ok(serverResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] BankRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (!await _bankService.Update(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            _logService.SaveLog(identity.companyid ?? 0, identity.employeeid, 0, "Update Bank", "Bank ID : " + requestDto.bankid.ToString(), 0);
            return Ok(serverResponse);
        }
    }
}
