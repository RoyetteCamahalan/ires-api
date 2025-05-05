using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.CashDisbursement;
using ires.Domain.DTO.PettyCash;
using ires.Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PettyCashController : ControllerBase
    {
        private readonly IPettyCashService _pettyCashService;
        private readonly IAccountService _accountService;

        public PettyCashController(IPettyCashService pettyCashService, IAccountService accountService)
        {
            _pettyCashService = pettyCashService;
            _accountService = accountService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<CashDisbursementViewModel>();
            var result = await _pettyCashService.GetDisbursementByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(int currentPage, DateTime startDate, DateTime endDate, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<CashDisbursementViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _pettyCashService.GetCashDisbursements(identity.companyid ?? 0, search ?? "", startDate, endDate);
            var paginator = new PaginatorDto<CashDisbursementViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("getaccounthistory")]
        public async Task<IActionResult> GetAccountHistory(long id, DateTime startDate, DateTime endDate)
        {
            var serverResponse = new ServerResponse<ICollection<PettyCashAccountHistoryViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _pettyCashService.GetAccountHistory(identity.companyid ?? 0, id, startDate, endDate);
            serverResponse.Data = result;
            return Ok(serverResponse);

        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CashDisbursementRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<CashDisbursementViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (requestDto.transtype == DisbursementTransType.transferout)
            {
                var office = await _accountService.GetOfficeByID(requestDto.accountid);
                if (office.pettycashbalance < requestDto.amount)
                {
                    serverResponse.Success = false;
                    serverResponse.Message = "Insufficient petty cash balance";
                    return BadRequest(serverResponse);
                }
            }
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            var result = await _pettyCashService.Create(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut("voiddisbursement")]
        public async Task<IActionResult> VoidDisbursement([FromBody] IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            serverResponse.Success = await _pettyCashService.VoidDisbursement(requestDto.id, false, identity.employeeid);
            serverResponse.Data = serverResponse.Success;
            if (!serverResponse.Success)
            {
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
        [HttpPost("recompute/{id}")]
        public async Task<IActionResult> Recompute(long id)
        {
            var serverResponse = new ServerResponse<bool>();
            await _pettyCashService.ReComputePettyCash(id);
            return Ok(serverResponse);
        }
    }
}
