using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpGet("getpayables")]
        public IActionResult GetPayables(long clientID, int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<PayableDto>>();
            var payableDtos= _paymentService.GetPayables(clientID, search ?? "");
            var paginator = new PaginatorDto<PayableDto>(currentPage);
            paginator.Paginate(payableDtos);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
    }
}
