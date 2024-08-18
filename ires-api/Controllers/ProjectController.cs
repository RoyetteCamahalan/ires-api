using AutoMapper;
using ires.AppService.Common;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.Project;
using ires.Domain.DTO.RentalUnit;
using ires.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController(
        IMapper _mapper,
        IProjectService _projectService,
        IRentalService _rentalService,
        IBillService _billService) : ControllerBase
    {

        [HttpGet("getrentalproperties")]
        public async Task<IActionResult> GetRentalProperties([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<RentalProjectViewModel>>
            {
                Data = await _projectService.GetRentalProperties(request)
            };
            return Ok(serverResponse);

        }

        [HttpGet("getrentalproperty/{id}")]
        public async Task<IActionResult> GetRentalProperty(long id)
        {
            var serverResponse = new ServerResponse<RentalProjectViewModel>();
            var result = await _projectService.GetProjectByIdAsync(id);
            if (result == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<RentalProjectViewModel>(result);
            return Ok(serverResponse);
        }
        [HttpPost("createrentalproperty")]
        public async Task<IActionResult> CreateRentalProperty([FromBody] RentalProjectRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<RentalProjectViewModel>();
            requestDto.projectypeid = ProjectType.Rental;
            serverResponse.Data = await _projectService.Create(_mapper.Map<ProjectRequestDto>(requestDto));
            return Ok(serverResponse);
        }
        [HttpPut("updaterentalproperty")]
        public async Task<IActionResult> UpdateRentalProperty([FromBody] RentalProjectRequestDto requestDto)
        {
            ;
            await _projectService.Update(_mapper.Map<ProjectRequestDto>(requestDto));
            return Ok(new ServerResponse<RentalProjectViewModel>());
        }




        [HttpGet("getrentalunits")]
        public async Task<IActionResult> GetRentalUnits([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<RentalUnitViewModel>>
            {
                Data = await _projectService.GetRentalUnits(request)
            };
            return Ok(serverResponse);
        }

        [HttpGet("getavailablerentalunits")]
        public async Task<IActionResult> GetAvailableRentalUnits([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<RentalUnitViewModel>>
            {
                Data = await _projectService.GetAvailableRentalUnits(request)
            };
            return Ok(serverResponse);
        }

        [HttpGet("getrentalunit/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRentalUnit(long id)
        {
            var serverResponse = new ServerResponse<RentalUnitViewModel>
            {
                Data = await _projectService.GetRentalUnitByIdAsync(id)
            };
            return Ok(serverResponse);
        }
        [HttpPost("createrentalunit")]
        public async Task<IActionResult> CreateRentalUnit([FromBody] RentalUnitRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<RentalUnitViewModel>();
            var companyPlan = await _billService.GetSubscriptionPlans();
            if (companyPlan.surveylimit > 0)
            {
                var rentalUnitCount = await _rentalService.CountActiveUnits();
                if (rentalUnitCount >= companyPlan.surveylimit)
                {
                    serverResponse.Success = false;
                    serverResponse.errorCode = 100;
                    serverResponse.message = $"Reached maximum number of rental units ({companyPlan.surveylimit}).Please upgrage you plan!";
                    return BadRequest(serverResponse);
                }
            }
            serverResponse.Data = await _projectService.CreateRentalUnit(requestDto);
            return Ok(serverResponse);
        }
        [HttpPut("updaterentalunit")]
        public async Task<IActionResult> UpdateRentalUnit([FromBody] RentalUnitRequestDto requestDto)
        {
            await _projectService.UpdateRentalUnit(requestDto);
            return Ok(new ServerResponse<bool>());
        }
    }
}
