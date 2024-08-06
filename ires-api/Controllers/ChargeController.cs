using ires.AppService.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.OtherCharge;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChargeController(IChargeService _chargeService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetSurveyCharges(long surveyID, int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<OtherChargeViewModel>>();
            var result = await _chargeService.GetOtherCharges(surveyID, search ?? "");
            var paginator = new PaginatorDto<OtherChargeViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
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
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] OtherChargeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            if (!await _chargeService.Update(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
    }
}
