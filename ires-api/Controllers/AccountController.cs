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
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet("getbankaccounts")]
        public IActionResult GetBankAccounts(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<BankAccountDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var bankAccounts = _accountService.GetBankAccounts(identity.companyid ?? 0, search ?? "");
            var paginator = new PaginatorDto<BankAccountDto>(currentPage);
            paginator.Paginate(_mapper.Map<List<BankAccountDto>>(bankAccounts));
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("getbankaccount/{id}")]
        [AllowAnonymous]
        public IActionResult Get(long id)
        {
            var serverResponse = new ServerResponse<BankAccountDto>();
            var bankAccount = _accountService.GetBankAccountByID(id);
            if (bankAccount == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<BankAccountDto>(bankAccount);
            return Ok(serverResponse);
        }

        [HttpPost("createbankaccount")]
        public IActionResult Post([FromBody] BankAccountRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BankAccountRequestDto>();
            if (_accountService.isBankAccountExist(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Bank account already exist";
                return BadRequest(serverResponse);
            }
            var result = _accountService.CreateBankAccount(_mapper.Map<BankAccount>(requestDto));
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<BankAccountRequestDto>(result);
            return Ok(serverResponse);
        }

        [HttpPut("updatebankaccount")]
        public IActionResult Put([FromBody] BankAccountRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BankAccountRequestDto>();
            var bankAccount = _accountService.UpdateBankAccount(requestDto);
            if (bankAccount == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
    }
}
