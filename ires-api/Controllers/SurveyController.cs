using ires.AppService.Common;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.Survey;
using ires.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController(ISurveyService _surveyService, IPaymentService _paymentService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<SurveyViewModel>>
            {
                Data = await _surveyService.GetSurveys(request)
            };
            return Ok(serverResponse);

        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<SurveyViewModel>
            {
                Data = await _surveyService.GetByID(id)
            };
            return Ok(serverResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SurveyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<SurveyViewModel>
            {
                Data = await _surveyService.Create(requestDto)
            };
            return Ok(serverResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SurveyRequestDto requestDto)
        {
            await _surveyService.Update(requestDto);
            return Ok(new ServerResponse<bool>());
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] SurveyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            if (requestDto.status == SurveyStatus.cancelled)
            {
                var paymentDetails = await _paymentService.GetSurveyPaymentDetails(requestDto.id);
                if (paymentDetails.Count > 0)
                {
                    serverResponse.Success = false;
                    serverResponse.message = "Please void payment: (";
                    foreach (var detail in paymentDetails)
                    {
                        serverResponse.message += detail.payment.receiptno + ", ";
                    }
                    serverResponse.message = serverResponse.message.Substring(0, serverResponse.message.Length - 2) + ")";
                    return BadRequest(serverResponse);
                }
            }
            await _surveyService.UpdateStatus(requestDto.id, requestDto.status);
            return Ok(serverResponse);
        }
    }
}
