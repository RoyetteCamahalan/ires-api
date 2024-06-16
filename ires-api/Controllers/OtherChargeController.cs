using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.OtherFee;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherChargeController : ControllerBase
    {
        private readonly IOtherChargeService _otherChargeService;

        public OtherChargeController(IOtherChargeService otherChargeService)
        {
            _otherChargeService = otherChargeService;
        }

        [HttpGet("getotherfees")]
        public async Task<IActionResult> GetOtherFees(int currentPage, bool viewAll, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<OtherFeeViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _otherChargeService.GetOtherFees(identity.companyid ?? 0, search ?? "", viewAll);
            var paginator = new PaginatorDto<OtherFeeViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);
        }

        [HttpGet("getotherfee/{id}")]
        public async Task<IActionResult> GetOtherFee(long id)
        {
            var serverResponse = new ServerResponse<OtherFeeViewModel>();
            var result = await _otherChargeService.GetOtherFee(id);
            if (result == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }
        [HttpPost("createotherfee")]
        public async Task<IActionResult> CreateOtherFee([FromBody] OtherFeeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<OtherFeeViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdby = identity.employeeid;
            var result = await _otherChargeService.CreateOtherFee(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }
        [HttpPut("updateotherfee")]
        public async Task<IActionResult> UpdateOtherFee([FromBody] OtherFeeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            if (!await _otherChargeService.UpdateOtherFee(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
    }
}
