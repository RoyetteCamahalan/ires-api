using ires.AppService.Common;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.OtherFee;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherChargeController(IOtherChargeService _otherChargeService) : ControllerBase
    {

        [HttpGet("getotherfees")]
        public async Task<IActionResult> GetOtherFees([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<OtherFeeViewModel>>
            {
                Data = await _otherChargeService.GetOtherFees(request)
            };
            return Ok(serverResponse);
        }

        [HttpGet("getotherfee/{id}")]
        public async Task<IActionResult> GetOtherFee(long id)
        {
            var serverResponse = new ServerResponse<OtherFeeViewModel>
            {
                Data = await _otherChargeService.GetOtherFee(id)
            };
            return Ok(serverResponse);
        }
        [HttpPost("createotherfee")]
        public async Task<IActionResult> CreateOtherFee([FromBody] OtherFeeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<OtherFeeViewModel>
            {
                Data = await _otherChargeService.CreateOtherFee(requestDto)
            };
            return Ok(serverResponse);
        }
        [HttpPut("updateotherfee")]
        public async Task<IActionResult> UpdateOtherFee([FromBody] OtherFeeRequestDto requestDto)
        {
            await _otherChargeService.UpdateOtherFee(requestDto);
            return Ok(new ServerResponse<bool>());
        }
    }
}
