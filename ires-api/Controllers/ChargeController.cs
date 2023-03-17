using AutoMapper;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChargeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChargeService _chargeService;

        public ChargeController(IMapper mapper, IChargeService chargeService)
        {
            _mapper = mapper;
            _chargeService = chargeService;
        }
        [HttpGet]
        public IActionResult GetSurveyCharges(long surveyID, int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<OtherChargeDto>>();
            List<OtherChargeDto> otherChargeDtos = _mapper.Map<List<OtherChargeDto>>(_chargeService.GetOtherCharges(surveyID, search ?? ""));
            var paginator = new PaginatorDto<OtherChargeDto>(currentPage);
            paginator.Paginate(otherChargeDtos);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(long id)
        {
            var serverResponse = new ServerResponse<OtherChargeDto>();
            var otherCharge = _chargeService.GetOtherChargeByID(id);
            if (otherCharge == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<OtherChargeDto>(otherCharge);
            return Ok(serverResponse);
        }

        [HttpPost]
        public IActionResult Post([FromBody] OtherChargeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<OtherChargeDto>();
            var result = _chargeService.Create(_mapper.Map<OtherCharge>(requestDto));
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<OtherChargeDto>(result);
            return Ok(serverResponse);
        }

        [HttpPut]
        public IActionResult Put([FromBody] OtherChargeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<OtherChargeDto>();
            var result = _chargeService.Update(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
    }
}
