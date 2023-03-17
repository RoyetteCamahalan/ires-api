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
    public class SurveyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISurveyService _surveyService;

        public SurveyController(IMapper mapper, ISurveyService surveyService)
        {
            _mapper = mapper;
            _surveyService = surveyService;
        }
        [HttpGet]
        public IActionResult Get(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<SurveyDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            List<SurveyDto> surveyDtos = _mapper.Map<List<SurveyDto>>(_surveyService.GetSurveys(identity.companyid ?? 0, search ?? ""));
            var paginator = new PaginatorDto<SurveyDto>(currentPage);
            paginator.Paginate(surveyDtos);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(long id)
        {
            var serverResponse = new ServerResponse<SurveyDto>();
            var survey = _surveyService.GetSurveyByID(id);
            if (survey == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<SurveyDto>(survey);
            return Ok(serverResponse);
        }

        [HttpPost]
        public IActionResult Post([FromBody] SurveyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<SurveyDto>();
            var result = _surveyService.Create(_mapper.Map<Survey>(requestDto));
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<SurveyDto>(result);
            return Ok(serverResponse);
        }

        [HttpPut]
        public IActionResult Put([FromBody] SurveyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<SurveyDto>();
            var employee = _surveyService.Update(requestDto);
            if (employee == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
    }
}
