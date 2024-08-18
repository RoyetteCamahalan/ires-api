using ires.AppService.Common;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Company;
using ires.Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController(IConfiguration _configuration, IBillService _billService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<BillViewModel>>
            {
                Data = await _billService.GetBills(request)
            };
            return Ok(serverResponse);

        }
        [HttpGet("getplans")]
        public async Task<IActionResult> GetPlans()
        {
            var serverResponse = new ServerResponse<ICollection<CompanyPlanViewModel>>();
            var plan = await _billService.GetSubscriptionPlans();
            serverResponse.Data = [plan];
            return Ok(serverResponse);
        }

        [HttpPost("processpayment")]
        public async Task<IActionResult> ProcessPayment(IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BillViewModel>
            {
                Data = await _billService.StartPayment(requestDto.id, new PayMongoConfig(_configuration))
            };
            return Ok(serverResponse);
        }

        [HttpPost("completepayment")]
        public async Task<IActionResult> CompletePayment(IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BillViewModel>();
            var bill = await _billService.CompletePayment(requestDto.id, new PayMongoConfig(_configuration));
            if (bill != null && bill.status == BillStatus.paid)
                serverResponse.Data = bill;
            else
            {
                serverResponse.Success = false;
                serverResponse.message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }

        [HttpPut("updatebillingcycle")]
        public IActionResult UpdateBillingCycle(RegisterCompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            _billService.UpdateBillingCycle(requestDto);
            serverResponse.Data = true;
            return Ok(serverResponse);
        }

        [HttpPut("upgradeplan")]
        public async Task<IActionResult> UpgradePlan(RegisterCompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            await _billService.UpgradePlan(requestDto.planid);
            serverResponse.Data = true;
            return Ok(serverResponse);
        }
        [HttpPost("getinvoicedocument/{billid}")]
        public async Task<IActionResult> GetInvoice(long billid)
        {
            var serverResponse = new ServerResponse<FileDataViewModel>();
            var data = await _billService.GenerateInvoice(billid);
            if (data == null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Failed to get invoice";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = data;
            return Ok(serverResponse);
        }
    }
}
