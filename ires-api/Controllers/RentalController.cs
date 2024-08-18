using ires.AppService.Common;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.RentalCharge;
using ires.Domain.DTO.RentalContract;
using ires.Domain.DTO.RentalContractDetail;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController(IRentalService _rentalService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RentalContractRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<RentalContractViewModel>
            {
                Data = await _rentalService.Create(requestDto)
            };
            return Ok(serverResponse);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RentalContractRequestDto requestDto)
        {
            await _rentalService.Update(requestDto);
            return Ok(new ServerResponse<bool>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<RentalContractViewModel>();
            var result = await _rentalService.Get(id);
            result.propertyList = await _rentalService.GetPropertiesAsString(result.contractid);
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpGet("getdetails/{id}")]
        public async Task<IActionResult> GetContractDetails(long id)
        {
            var serverResponse = new ServerResponse<ICollection<RentalContractDetailViewModel>>
            {
                Data = await _rentalService.GetDetails(id)
            };
            return Ok(serverResponse);
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<RentalContractViewModel>>();
            var result = await _rentalService.GetAll(request);
            foreach (var data in result.data)
                data.propertyList = await _rentalService.GetPropertiesAsString(data.contractid);
            serverResponse.Data = result;
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
            var serverResponse = new ServerResponse<ICollection<RentalHistoryViewModel>>
            {
                Data = await _rentalService.GetAccountHistory(id)
            };
            return Ok(serverResponse);
        }
        [HttpGet("getsoa/{id}")]
        public async Task<IActionResult> GetInvoice(long id)
        {
            var serverResponse = new ServerResponse<FileDataViewModel>();
            var data = await _rentalService.GenerateSOA(id);
            if (data == null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Failed to get invoice";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = data;
            return Ok(serverResponse);
        }


        [HttpGet("getrentalcharge/{id}")]
        public async Task<IActionResult> GetRentalCharge(long id)
        {
            var serverResponse = new ServerResponse<RentalChargeViewModel>
            {
                Data = await _rentalService.GetRentalCharge(id)
            };
            return Ok(serverResponse);
        }
        [HttpPost("postothercharge")]
        public async Task<IActionResult> PostOtherCharge([FromBody] RentalChargeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<RentalChargeViewModel>
            {
                Data = await _rentalService.CreateOtherCharge(requestDto)
            };
            return Ok(serverResponse);
        }
        [HttpPut("updateothercharge")]
        public async Task<IActionResult> UpdateOtherCharge([FromBody] RentalChargeRequestDto requestDto)
        {
            await _rentalService.UpdateOtherCharge(requestDto);
            return Ok(new ServerResponse<bool>());
        }
        [HttpDelete("deleteothercharge/{id}")]
        public async Task<IActionResult> DeleteOtherCharge(long id)
        {
            var serverResponse = new ServerResponse<bool>();
            if (await _rentalService.RentalChageHasPayment(id))
            {
                serverResponse.Success = false;
                serverResponse.message = "There is a payment for this charge, please void payment first!";
                return BadRequest(serverResponse);
            }
            await _rentalService.DeleteOtherCharge(id);
            return Ok(serverResponse);
        }
        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus(RentalTerminateRequestDto requestDto)
        {
            await _rentalService.UpdateContractStatus(requestDto);
            return Ok(new ServerResponse<bool>());
        }

        [HttpPost("sendsoa")]
        public async Task<IActionResult> SendSoa(SendMailRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            if (!Utility.IsEmailValid(requestDto.email))
            {
                serverResponse.Success = false;
                serverResponse.message = "Client email is invalid";
                return BadRequest(serverResponse);
            }
            if (!await _rentalService.SendSOA(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.message = "Failed to generate SOA";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = true;
            return Ok(serverResponse);
        }
    }
}