using AutoMapper;
using ires.AppService.DTO.Project;
using ires.AppService.DTO.RentalUnit;
using ires.Core.Commands.Project;
using ires.Core.Queries.Agent;
using ires.Core.Queries.Project;
using ires.Core.ViewModels;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.Enumerations;
using ires.Domain.Models;
using ires_api.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController(IMediator _mediator, IMapper mapper, IProjectService _projectService, IRentalService _rentalService, IBillService _billService) 
        : BaseController(_mediator, mapper)
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            return await Handle<PaginatedResult<ProjectViewModel>>(new GetProjectsQuery(request));
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> Get(Guid guid)
        {
            return await Handle<ProjectViewModel>(new GetProjectbyIdQuery(guid));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProjectRequestDto request)
        {
            return await Handle<CreateProjectRequestDto, CreateProjectCommand, ProjectViewModel>(request);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateProjectRequestDto request)
        {
            return await Handle<UpdateProjectRequestDto, UpdateProjectCommand, object>(request);
        }

        [HttpGet("getrentalproperties")]
        public async Task<IActionResult> GetRentalProperties(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<RentalProjectViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _projectService.GetRentalProperties(search ?? "");
            var paginator = new PaginatorDto<RentalProjectViewModel>(currentPage);
            paginator.Paginate(_mapper.Map<IEnumerable<RentalProjectViewModel>>(result));
            foreach (var item in paginator.data)
            {
                var property = result.Where(x => x.propertyid == item.propertyid).First();
                item.noofunits = property.rentalProperties.Count;
                item.noofoccupiedunits = property.rentalProperties.Where(x => x.status == RentalPropertyStatus.Occupied).Count();
            }
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("getrentalproperty/{id}")]
        public async Task<IActionResult> GetRentalProperty(long id)
        {
            var serverResponse = new ServerResponse<RentalProjectViewModel>();
            var result = await _projectService.GetProjectByIdAsync(id);
            serverResponse.Data = _mapper.Map<RentalProjectViewModel>(result);
            return Ok(serverResponse);
        }
        [HttpPost("createrentalproperty")]
        public async Task<IActionResult> CreateRentalProperty([FromBody] RentalProjectRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<RentalProjectViewModel>();
            requestDto.projectypeid = ProjectType.Rental;
            var result = await _projectService.Create(_mapper.Map<Project>(requestDto));
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<RentalProjectViewModel>(result);
            return Ok(serverResponse);
        }
        [HttpPut("updaterentalproperty")]
        public async Task<IActionResult> UpdateRentalProperty([FromBody] RentalProjectRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<RentalProjectViewModel>();
            await _projectService.Update(_mapper.Map<Project>(requestDto));
            return Ok(serverResponse);
        }




        [HttpGet("getrentalunits")]
        public async Task<IActionResult> GetRentalUnits(long projectID, int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<ires.Core.ViewModels.RentalUnitViewModel>>();
            var result = await _projectService.GetRentalUnits(projectID, search ?? "");
            var paginator = new PaginatorDto<ires.Core.ViewModels.RentalUnitViewModel>(currentPage);
            paginator.Paginate(_mapper.Map<IEnumerable<ires.Core.ViewModels.RentalUnitViewModel>>(result));
            foreach (var item in paginator.data)
            {
                if (item.status == RentalPropertyStatus.Occupied)
                {
                    var contract = await _rentalService.GetContractByUnit(item.propertyid);
                    item.tenant = contract.client.fullname;
                }
            }
            serverResponse.Data = paginator;
            return Ok(serverResponse);
        }

        [HttpGet("getavailablerentalunits")]
        public async Task<IActionResult> GetAvailableRentalUnits(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<ires.Core.ViewModels.RentalUnitViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _projectService.GetAvailableRentalUnits(search ?? "");
            var paginator = new PaginatorDto<ires.Core.ViewModels.RentalUnitViewModel>(currentPage);
            paginator.Paginate(_mapper.Map<IEnumerable<ires.Core.ViewModels.RentalUnitViewModel>>(result));
            serverResponse.Data = paginator;
            return Ok(serverResponse);
        }

        [HttpGet("getrentalunit/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRentalUnit(long id)
        {
            var serverResponse = new ServerResponse<ires.Core.ViewModels.RentalUnitViewModel>();
            var result = await _projectService.GetRentalUnitByIdAsync(id);
            if (result == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<ires.Core.ViewModels.RentalUnitViewModel>(result);
            return Ok(serverResponse);
        }
        [HttpPost("createrentalunit")]
        public async Task<IActionResult> CreateRentalUnit([FromBody] RentalUnitRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<ires.Core.ViewModels.RentalUnitViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var companyPlan = await _billService.GetSubscriptionPlans(identity.companyid ?? 0);
            if (companyPlan.surveylimit > 0)
            {
                var rentalUnitCount = await _rentalService.CountActiveUnits(identity.companyid ?? 0);
                if (rentalUnitCount >= companyPlan.surveylimit)
                {
                    serverResponse.Success = false;
                    serverResponse.errorCode = 100;
                    serverResponse.Message = $"Reached maximum number of rental units ({companyPlan.surveylimit}).Please upgrage you plan!";
                    return BadRequest(serverResponse);
                }
            }
            var unit = _mapper.Map<RentalUnit>(requestDto);
            if (requestDto.isactive)
                unit.status = RentalPropertyStatus.Vacant;
            else
                unit.status = RentalPropertyStatus.Inactive;
            var result = await _projectService.CreateRentalUnit(unit);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<ires.Core.ViewModels.RentalUnitViewModel>(result);
            return Ok(serverResponse);
        }
        [HttpPut("updaterentalunit")]
        public async Task<IActionResult> UpdateRentalUnit([FromBody] RentalUnitRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<ires.Core.ViewModels.RentalUnitViewModel>();
            var unit = _mapper.Map<RentalUnit>(requestDto);
            unit.status = requestDto.isactive ? RentalPropertyStatus.Vacant : RentalPropertyStatus.Inactive;
            await _projectService.UpdateRentalUnit(unit);
            return Ok(serverResponse);
        }
    }
}
