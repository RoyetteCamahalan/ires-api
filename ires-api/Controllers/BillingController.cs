using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Attachment;
using ires.Domain.DTO.Company;
using ires.Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBillService _billService;

        public BillingController(IConfiguration configuration, IBillService billService)
        {
            _configuration = configuration;
            _billService = billService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int currentPage, int filter)
        {
            var serverResponse = new ServerResponse<PaginatorDto<BillViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var billDtos = await _billService.GetBills(identity.companyid ?? 0, filter);
            var paginator = new PaginatorDto<BillViewModel>(currentPage);
            paginator.Paginate(billDtos);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpGet("getplans")]
        public async Task<IActionResult> GetPlans()
        {
            var serverResponse = new ServerResponse<ICollection<CompanyPlanViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var plan = await _billService.GetSubscriptionPlans(identity.companyid ?? 0);
            serverResponse.Data = new List<CompanyPlanViewModel> { plan };
            return Ok(serverResponse);
        }

        [HttpPost("processpayment")]
        public async Task<IActionResult> ProcessPayment(IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BillViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var bill = await _billService.StartPayment(identity.companyid ?? 0, requestDto.id, new PayMongoConfig(_configuration));
            serverResponse.Data = bill;
            return Ok(serverResponse);
        }

        [HttpPost("completepayment")]
        public async Task<IActionResult> CompletePayment(IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BillViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var bill = await _billService.CompletePayment(identity.companyid ?? 0, requestDto.id, new PayMongoConfig(_configuration));
            if (bill != null && bill.status == BillStatus.paid)
                serverResponse.Data = bill;
            else
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }

        [HttpPut("updatebillingcycle")]
        public IActionResult UpdateBillingCycle(RegisterCompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.id = identity.companyid ?? 0;
            requestDto.updatedbyid = identity.employeeid;
            _billService.UpdateBillingCycle(requestDto);
            serverResponse.Data = true;
            return Ok(serverResponse);
        }

        [HttpPut("upgradeplan")]
        public async Task<IActionResult> UpgradePlan(RegisterCompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (!await _billService.UpgradePlan(identity.companyid ?? 0, requestDto.planid, identity.employeeid))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = true;
            return Ok(serverResponse);
        }
        [HttpPost("getinvoicedocument/{billid}")]
        public async Task<IActionResult> GetInvoice(long billid)
        {
            var serverResponse = new ServerResponse<FileViewModel>();
            var data = await _billService.GenerateInvoice(billid);
            if (data == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Failed to get invoice";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = data;
            return Ok(serverResponse);
        }
    }
}
