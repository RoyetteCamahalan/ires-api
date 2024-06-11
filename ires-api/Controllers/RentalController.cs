using ires_api.DTO;
using ires_api.DTO.RentalContract;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly ILogService _logService;

        public RentalController(IRentalService rentalService, ILogService logService)
        {
            _rentalService = rentalService;
            _logService = logService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RentalContractRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<RentalContractViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            var result = await _rentalService.Create(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            _logService.SaveLog(identity.companyid ?? 0, identity.employeeid, 0, "Create Rental Contract", "Create New Record : " + result.contractid.ToString(), 0);
            return Ok(serverResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<RentalContractViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _rentalService.Get(identity.companyid ?? 0, id);
            if (result == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }
        [HttpGet]
        public async Task<IActionResult> Get(int currentPage, int filterByID, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<RentalContractViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _rentalService.GetAll(identity.companyid ?? 0, search ?? "", filterByID);
            var paginator = new PaginatorDto<RentalContractViewModel>(currentPage);
            paginator.Paginate(result);
            foreach (var data in paginator.data)
            {
                long lastProjectID = 0;
                var strings = new List<string>();
                var details = await _rentalService.GetProperties(data.contractid);
                foreach (var property in details)
                {
                    if (lastProjectID != property.projectid)
                    {
                        if (lastProjectID > 0)
                            strings[^1] += ")";
                        lastProjectID = property.projectid;
                        strings.Add(property.project.propertyname + "(" + property.propertyname);
                    }
                    else
                        strings.Add("," + property.propertyname);
                }
                data.propertyList = string.Join(" | ", strings) + ")";
            }
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
    }
}
