using ires_api.DTO;
using ires_api.DTO.Survey;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly IPaymentService _paymentService;

        public SurveyController(ISurveyService surveyService, IPaymentService paymentService)
        {
            _surveyService = surveyService;
            _paymentService = paymentService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<SurveyViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _surveyService.GetSurveys(identity.companyid ?? 0, search ?? "");
            var paginator = new PaginatorDto<SurveyViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<SurveyViewModel>();
            var result = await _surveyService.GetByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SurveyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<SurveyViewModel>();
            var result = await _surveyService.Create(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SurveyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            if (!await _surveyService.Update(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] SurveyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            if (requestDto.status == Constants.SurveyStatus.cancelled)
            {
                var paymentDetails = await _paymentService.GetSurveyPaymentDetails(requestDto.id);
                if (paymentDetails.Count > 0)
                {
                    serverResponse.Success = false;
                    serverResponse.Message = "Please void payment: (";
                    foreach (var detail in paymentDetails)
                    {
                        serverResponse.Message += detail.payment.receiptno + ", ";
                    }
                    serverResponse.Message = serverResponse.Message.Substring(0, serverResponse.Message.Length - 2) + ")";
                    return BadRequest(serverResponse);
                }
            }
            if (!await _surveyService.UpdateStatus(requestDto.id, requestDto.status))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Survey not found";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
    }
}
