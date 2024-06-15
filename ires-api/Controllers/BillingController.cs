using ires.Domain.Contracts;
using ires.Domain.DTO;
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
        private readonly ILogService _logService;

        public BillingController(IConfiguration configuration, IBillService billService, ILogService logService)
        {
            _configuration = configuration;
            _billService = billService;
            _logService = logService;
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
            var serverResponse = new ServerResponse<CompanyViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = await _billService.GetSubscriptionPlans(identity.companyid ?? 0);
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
        public IActionResult UpdateBillingCycle(CompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            _billService.UpdateBillingCycle(identity.companyid ?? 0, requestDto.billingcycle);
            _logService.SaveLog(identity.companyid ?? 0, identity.employeeid, 0, "Billing Cycle", "Updated Billing Cycle : " + (requestDto.billingcycle == 1 ? "Monthly" : "Yearly"), 1);
            serverResponse.Data = true;
            return Ok(serverResponse);
        }

        [HttpPut("upgradeplan")]
        public async Task<IActionResult> UpgradePlan(CompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            if (!await _billService.UpgradePlan(identity.companyid ?? 0, requestDto.planid))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            _logService.SaveLog(identity.companyid ?? 0, identity.employeeid, 0, "Subscription Upgrade", "Subscription Upgrade : " + requestDto.planid.ToString(), 1);
            serverResponse.Data = true;
            return Ok(serverResponse);
        }
    }
}
