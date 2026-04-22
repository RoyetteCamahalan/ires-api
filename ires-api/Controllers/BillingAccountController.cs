using ires.Domain.Contracts;
using ires.Domain.DTO.BillingAccount;
using ires.Domain.DTO.BillingPayment;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingAccountController : ControllerBase
    {
        private readonly IBillingAccountService _billingAccountService;

        public BillingAccountController(IBillingAccountService billingAccountService)
        {
            _billingAccountService = billingAccountService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _billingAccountService.GetBillingAccountById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(bool viewAll = false, string search = "")
        {
            var result = await _billingAccountService.GetBillingAccounts(viewAll, search);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BillingAccountRequestDto requestDto)
        {
            var result = await _billingAccountService.CreateBillingAccount(requestDto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BillingAccountRequestDto requestDto)
        {
            var success = await _billingAccountService.UpdateBillingAccount(requestDto);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> SetStatus(long id, [FromQuery] bool isActive)
        {
            var success = await _billingAccountService.SetBillingAccountStatus(id, isActive);
            if (!success) return NotFound();
            return Ok();
        }

        // Billing Payments

        [HttpGet("{billingAccountId}/payments")]
        public async Task<IActionResult> GetPayments(long billingAccountId)
        {
            var result = await _billingAccountService.GetBillingPayments(billingAccountId);
            return Ok(result);
        }

        [HttpGet("payments/{id}")]
        public async Task<IActionResult> GetPayment(long id)
        {
            var result = await _billingAccountService.GetBillingPaymentById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("payments")]
        public async Task<IActionResult> PostPayment([FromBody] BillingPaymentRequestDto requestDto)
        {
            var result = await _billingAccountService.PostPayment(requestDto);
            return Ok(result);
        }

        [HttpPatch("payments/{id}/void")]
        public async Task<IActionResult> VoidPayment(long id)
        {
            var success = await _billingAccountService.VoidPayment(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
