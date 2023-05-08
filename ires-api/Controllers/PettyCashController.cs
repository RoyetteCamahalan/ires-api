
using AutoMapper;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PettyCashController : ControllerBase
    {
        private readonly IPettyCashService _pettyCashService;
        private readonly IAccountService _accountService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public PettyCashController(IPettyCashService pettyCashService, IAccountService accountService, ILogService logService, IMapper mapper)
        {
            _pettyCashService = pettyCashService;
            _accountService = accountService;
            _logService = logService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<CashDisbursementDto>();
            var result = await _pettyCashService.GetDisbursementByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<CashDisbursementDto>(result);
            return Ok(serverResponse);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(int currentPage, DateTime startDate, DateTime endDate, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<CashDisbursementDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _pettyCashService.GetCashDisbursements(identity.companyid ?? 0, search ?? "", startDate, endDate);
            var paginator = new PaginatorDto<CashDisbursementDto>(currentPage);
            paginator.Paginate(_mapper.Map<List<CashDisbursementDto>>(result));
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CashDisbursementRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<CashDisbursementDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (requestDto.transtype == Constants.DisbursementTransType.transferout)
            {
                var office = _accountService.GetOfficeByID(requestDto.accountid);
                if (office.pettycashbalance < requestDto.amount)
                {
                    serverResponse.Success = false;
                    serverResponse.Message = "Insufficient petty cash balance";
                    return BadRequest(serverResponse);
                }
            }
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            var result = await _pettyCashService.Create(_mapper.Map<CashDisbursement>(requestDto));
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<CashDisbursementDto>(result);
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Petty Cash Disbursement", "Create New Record : " + result.disbursementid, 0);
            return Ok(serverResponse);
        }

        [HttpPut("voiddisbursement")]
        public async Task<IActionResult> VoidDisbursement([FromBody] IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            serverResponse.Success = await _pettyCashService.VoidDisbursement(requestDto.id, false);
            serverResponse.Data = serverResponse.Success;
            if (!serverResponse.Success)
            {
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            _logService.SaveLog(identity.companyid ?? 0, identity.employeeid, 0, "Petty Cash Disbursement", "Void Record : " + requestDto.id, 0);
            return Ok(serverResponse);
        }
    }
}
