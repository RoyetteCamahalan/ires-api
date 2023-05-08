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
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, ILogService logService, IMapper mapper)
        {
            _accountService = accountService;
            _logService = logService;
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
            var identity = IdentityProfile.getIdentity(this.HttpContext);
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
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Create New Bank Account", "Account : " + result.accountid + '-' + requestDto.accountname, 0);
            return Ok(serverResponse);
        }

        [HttpPut("updatebankaccount")]
        public IActionResult Put([FromBody] BankAccountRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BankAccountRequestDto>();
            var result = _accountService.UpdateBankAccount(requestDto);
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<BankAccountRequestDto>(result);
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Updated Bank Account", "Account ID: " + result.accountid + '-' + requestDto.accountname, 0);
            return Ok(serverResponse);
        }





        [HttpGet("getoffice")]
        public IActionResult GetOffice(long id)
        {
            var serverResponse = new ServerResponse<OfficeDto>();
            var result = _accountService.GetOfficeByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<OfficeDto>(result);
            return Ok(serverResponse);

        }

        [HttpGet("getoffices")]
        public async Task<IActionResult> GetOffices(int currentPage, bool viewAll, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<OfficeDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var result = await _accountService.GetOffices(identity.companyid ?? 0, search ?? "", viewAll);
            var paginator = new PaginatorDto<OfficeDto>(currentPage);
            paginator.Paginate(_mapper.Map<List<OfficeDto>>(result));
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost("createoffice")]
        public IActionResult CreateOffice([FromBody] OfficeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<OfficeDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = _accountService.GetOfficeByName(requestDto.companyid, requestDto.accountname);
            if (result != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Bank already registered.";
                return BadRequest(serverResponse);
            }
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            result = _accountService.CreateOffice(_mapper.Map<Office>(requestDto));
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<OfficeDto>(result);
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Office", "Create New Office : " + result.accountid + "-" + requestDto.accountname, 0);
            return Ok(serverResponse);
        }

        [HttpPut("updateoffice")]
        public IActionResult UpdateOffice([FromBody] OfficeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<OfficeDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            var result = _accountService.UpdateOffice(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<OfficeDto>(result);
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Office", "Update Office ID : " + requestDto.accountid, 0);
            return Ok(serverResponse);
        }
    }
}
