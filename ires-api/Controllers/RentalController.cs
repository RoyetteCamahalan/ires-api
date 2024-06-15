using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.RentalContract;
using ires.Domain.DTO.RentalContractDetail;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RentalContractRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (!await _rentalService.Update(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            _logService.SaveLog(identity.companyid ?? 0, identity.employeeid, 0, "Update Rental Contract", "Update Record : " + requestDto.contractid.ToString(), 0);
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
            result.propertyList = await _rentalService.GetPropertiesAsString(result.contractid);
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpGet("getdetails/{id}")]
        public async Task<IActionResult> GetContractDetails(long id)
        {
            var serverResponse = new ServerResponse<ICollection<RentalContractDetailViewModel>>();
            var result = await _rentalService.GetDetails(id);
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
                data.propertyList = await _rentalService.GetPropertiesAsString(data.contractid);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpGet("getaccounthistory/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAccountHistory(long id)
        {
            var serverResponse = new ServerResponse<ICollection<RentalHistoryViewModel>>();
            var result = await _rentalService.GetAccountHistory(id, 1);
            serverResponse.Data = result;
            return Ok(serverResponse);
        }
    }
}
