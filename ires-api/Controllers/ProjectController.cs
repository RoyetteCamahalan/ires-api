using AutoMapper;
using ires_api.DTO;
using ires_api.DTO.Project;
using ires_api.DTO.RentalUnit;
using ires_api.Enumerations;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly ILogService _logService;

        public ProjectController(IMapper mapper, IProjectService projectService, ILogService logService)
        {
            _mapper = mapper;
            _projectService = projectService;
            _logService = logService;
        }

        [HttpGet("getrentalproperties")]
        public async Task<IActionResult> GetRentalProperties(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<RentalProjectViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _projectService.GetRentalProperties(identity.companyid ?? 0, search ?? "");
            var paginator = new PaginatorDto<RentalProjectViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
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
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.projectypeid = ProjectType.Rental;
            var result = await _projectService.Create(_mapper.Map<ProjectRequestDto>(requestDto));
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<RentalProjectViewModel>(result);
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Create Rental Property", "Create New Record : " + result.propertyname, 0);
            return Ok(serverResponse);
        }
        [HttpPut("updaterentalproperty")]
        public async Task<IActionResult> UpdateRentalProperty([FromBody] RentalProjectRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<RentalProjectViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var data = await _projectService.Update(_mapper.Map<ProjectRequestDto>(requestDto));
            if (data == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            _logService.SaveLog(data.companyid, identity.employeeid, 0, "Update Rental Property", "Update Record : " + data.propertyname, 0);
            return Ok(serverResponse);
        }




        [HttpGet("getrentalunits")]
        public async Task<IActionResult> GetRentalUnits(long projectID, int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<RentalUnitViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _projectService.GetRentalUnits(identity.companyid ?? 0, projectID, search ?? "");
            var paginator = new PaginatorDto<RentalUnitViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);
        }

        [HttpGet("getavailablerentalunits")]
        public async Task<IActionResult> GetAvailableRentalUnits(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<RentalUnitViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _projectService.GetAvailableRentalUnits(identity.companyid ?? 0, search ?? "");
            var paginator = new PaginatorDto<RentalUnitViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);
        }

        [HttpGet("getrentalunit/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRentalUnit(long id)
        {
            var serverResponse = new ServerResponse<RentalUnitViewModel>();
            var result = await _projectService.GetRentalUnitByIdAsync(id);
            if (result == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<RentalUnitViewModel>(result);
            return Ok(serverResponse);
        }
        [HttpPost("createrentalunit")]
        public async Task<IActionResult> CreateRentalUnit([FromBody] RentalUnitRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<RentalUnitViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _projectService.CreateRentalUnit(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<RentalUnitViewModel>(result);
            _logService.SaveLog(identity.companyid ?? 0, identity.employeeid, 0, "Create Rental Unit", "Create New Record : " + result.propertyid.ToString() + "-" + result.propertyname, 0);
            return Ok(serverResponse);
        }
        [HttpPut("updaterentalunit")]
        public async Task<IActionResult> UpdateRentalUnit([FromBody] RentalUnitRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<RentalUnitViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (!(await _projectService.UpdateRentalUnit(requestDto)))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            _logService.SaveLog(identity.companyid ?? 0, identity.employeeid, 0, "Update Rental Unit", "Update Record : " + requestDto.propertyid.ToString() + "-" + requestDto.propertyname, 0);
            return Ok(serverResponse);
        }
    }
}
