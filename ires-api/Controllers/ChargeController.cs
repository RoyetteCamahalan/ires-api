using ires.AppService.Common;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.OtherCharge;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChargeController(IChargeService _chargeService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetSurveyCharges(long surveyID, [FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<OtherChargeViewModel>>
            {
                Data = await _chargeService.GetOtherCharges(surveyID, request)
            };
            return Ok(serverResponse);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<OtherChargeViewModel>();
            var otherCharge = await _chargeService.GetOtherChargeByID(id);
            if (otherCharge == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = otherCharge;
            return Ok(serverResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OtherChargeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<OtherChargeViewModel>();
            var result = await _chargeService.Create(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] OtherChargeRequestDto requestDto)
        {
            await _chargeService.Update(requestDto);
            return Ok(new ServerResponse<bool>());
        }
    }
}
