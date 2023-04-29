using AutoMapper;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBillService _billService;
        private readonly ILogService _logService;

        public BillingController(IMapper mapper, IBillService billService, ILogService logService)
        {
            _mapper = mapper;
            _billService = billService;
            _logService = logService;
        }
        [HttpGet]
        public IActionResult Get(int currentPage, int filter)
        {
            var serverResponse = new ServerResponse<PaginatorDto<BillDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            List<BillDto> billDtos = _mapper.Map<List<BillDto>>(_billService.GetBills(identity.companyid ?? 0, filter));
            var paginator = new PaginatorDto<BillDto>(currentPage);
            paginator.Paginate(billDtos);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpGet("getplans")]
        public IActionResult GetPlans()
        {
            var serverResponse = new ServerResponse<PaginatorDto<CompanyPlanDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            List<CompanyPlanDto> companyPlanDtos = _mapper.Map<List<CompanyPlanDto>>(_billService.GetSubscriptionPlans(identity.companyid ?? 0));
            var paginator = new PaginatorDto<CompanyPlanDto>(0);
            paginator.Paginate(companyPlanDtos);
            serverResponse.Data = paginator;
            return Ok(serverResponse);
        }

        [HttpPost("processpayment")]
        public IActionResult ProcessPayment(IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BillDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var bill = _billService.StartPayment(identity.companyid ?? 0, requestDto.id);
            serverResponse.Data = _mapper.Map<BillDto>(bill);
            return Ok(serverResponse);
        }

        [HttpPost("completepayment")]
        public IActionResult CompletePayment(IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BillDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var bill = _billService.CompletePayment(identity.companyid ?? 0, requestDto.id);
            if (bill != null && bill.status == Constants.BillStatus.paid)
                serverResponse.Data = _mapper.Map<BillDto>(bill);
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
        public IActionResult UpgradePlan(CompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            bool updated = _billService.UpgradePlan(identity.companyid ?? 0, requestDto.planid);
            if (!updated)
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
