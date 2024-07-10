using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Attachment;
using ires.Domain.DTO.RentalCharge;
using ires.Domain.DTO.RentalContract;
using ires.Domain.DTO.RentalContractDetail;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalController(IRentalService rentalService)
        {
            _rentalService = rentalService;
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
            return Ok(serverResponse);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RentalContractRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            if (!await _rentalService.Update(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
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
        [HttpGet("recompute/{id}")]
        public async Task<IActionResult> RecomputeContract(long id)
        {
            var serverResponse = new ServerResponse<bool>();
            await _rentalService.RecomputeContract(id);
            return Ok(serverResponse);
        }
        [HttpGet("getaccounthistory/{id}")]
        public async Task<IActionResult> GetAccountHistory(long id)
        {
            var serverResponse = new ServerResponse<ICollection<RentalHistoryViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _rentalService.GetAccountHistory(identity.companyid ?? 0, id);
            serverResponse.Data = result;
            return Ok(serverResponse);
        }
        [HttpGet("getsoa/{id}")]
        public async Task<IActionResult> GetInvoice(long id)
        {
            var serverResponse = new ServerResponse<FileViewModel>();
            var data = await _rentalService.GenerateSOA(id);
            if (data == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Failed to get invoice";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = data;
            return Ok(serverResponse);
        }


        [HttpGet("getrentalcharge/{id}")]
        public async Task<IActionResult> GetRentalCharge(long id)
        {
            var serverResponse = new ServerResponse<RentalChargeViewModel>();
            var result = await _rentalService.GetRentalCharge(id);
            serverResponse.Data = result;
            return Ok(serverResponse);
        }
        [HttpPost("postothercharge")]
        public async Task<IActionResult> PostOtherCharge([FromBody] RentalChargeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<RentalChargeViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.createdbyid = identity.employeeid;
            var result = await _rentalService.CreateOtherCharge(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }
        [HttpPut("updateothercharge")]
        public async Task<IActionResult> UpdateOtherCharge([FromBody] RentalChargeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            if (!await _rentalService.UpdateOtherCharge(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
        [HttpDelete("deleteothercharge/{id}")]
        public async Task<IActionResult> DeleteOtherCharge(long id)
        {
            var serverResponse = new ServerResponse<bool>();
            if (await _rentalService.RentalChageHasPayment(id))
            {
                serverResponse.Success = false;
                serverResponse.Message = "There is a payment for this charge, please void payment first!";
                return BadRequest(serverResponse);
            }
            if (!await _rentalService.DeleteOtherCharge(id))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus(RentalTerminateRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            if (!await _rentalService.UpdateContractStatus(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }

        [HttpPost("sendsoa")]
        public async Task<IActionResult> SendSoa(SendMailRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.createdbyid = identity.employeeid;
            if (!Utility.IsEmailValid(requestDto.email))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Client email is invalid";
                return BadRequest(serverResponse);
            }
            if (!await _rentalService.SendSOA(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Failed to generate SOA";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = true;
            return Ok(serverResponse);
        }
    }
}
