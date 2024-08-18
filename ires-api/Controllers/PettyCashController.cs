using ires.AppService.Common;
using ires.Domain.Common;
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
    public class PettyCashController(IPettyCashService _pettyCashService, IAccountService _accountService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<CashDisbursementViewModel>();
            var result = await _pettyCashService.GetDisbursementByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<CashDisbursementViewModel>>
            {
                Data = await _pettyCashService.GetCashDisbursements(request)
            };
            return Ok(serverResponse);

        }

        [HttpGet("getaccounthistory")]
        public async Task<IActionResult> GetAccountHistory(long id, DateTime startDate, DateTime endDate)
        {
            var serverResponse = new ServerResponse<ICollection<PettyCashAccountHistoryViewModel>>();
            var result = await _pettyCashService.GetAccountHistory(id, startDate, endDate);
            serverResponse.Data = result;
            return Ok(serverResponse);

        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CashDisbursementRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<CashDisbursementViewModel>();
            if (requestDto.transtype == DisbursementTransType.transferout)
            {
                var office = await _accountService.GetOfficeByID(requestDto.accountid);
                if (office.pettycashbalance < requestDto.amount)
                {
                    serverResponse.Success = false;
                    serverResponse.message = "Insufficient petty cash balance";
                    return BadRequest(serverResponse);
                }
            }
            var result = await _pettyCashService.Create(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut("voiddisbursement")]
        public async Task<IActionResult> VoidDisbursement([FromBody] IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            await _pettyCashService.VoidDisbursement(requestDto.id, false);
            return Ok(serverResponse);
        }
    }
}
