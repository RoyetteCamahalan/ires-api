using ires.AppService.Common;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.Payment;
using ires.Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentService _paymentService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<PaymentViewModel>>
            {
                Data = await _paymentService.GetPayments(request)
            };
            return Ok(serverResponse);

        }

        [HttpGet("getcollection")]
        public async Task<IActionResult> GetCollection(DateTime startDate, DateTime endDate)
        {
            var serverResponse = new ServerResponse<PaymentCollectionViewModel>();
            var paymentList = await _paymentService.GetPayments(new PaginationRequest { startDate = startDate, endDate = endDate });
            var payments = paymentList.data.Where(x => x.status != PaymentStatus.@void).ToList();
            var collection = new PaymentCollectionViewModel
            {
                payments = payments,
                totalCash = payments.Where(x => x.paymentmode == PaymentMode.cash).Select(x => x.totalamount).Sum(),
                totalCheck = payments.Where(x => x.paymentmode == PaymentMode.check).Select(x => x.totalamount).Sum(),
                totalBankTransfer = payments.Where(x => x.paymentmode == PaymentMode.bankTransfer).Select(x => x.totalamount).Sum(),
                totalWallet = payments.Where(x => x.paymentmode == PaymentMode.eWallet).Select(x => x.totalamount).Sum()
            };
            collection.totalPayment = collection.totalCash + collection.totalCheck + collection.totalBankTransfer + collection.totalWallet;
            serverResponse.Data = collection;
            return Ok(serverResponse);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<PaymentViewModel>();
            var payment = await _paymentService.GetPayment(id);
            payment.payables = await _paymentService.GetPaymentDetailsAsPayables(payment.paymentid);
            serverResponse.Data = payment;
            return Ok(serverResponse);

        }
        [HttpGet("getpayables")]
        public async Task<IActionResult> GetPayables(long clientID, [FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<PayableViewModel>>
            {
                Data = await _paymentService.GetPayables(clientID, request)
            };
            return Ok(serverResponse);

        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<PaymentViewModel>();
            var payableDtos = await _paymentService.GetPayables(requestDto.custid, new PaginationRequest());
            if (requestDto.payables.Count == 0)
            {
                serverResponse.Success = false;
                serverResponse.message = "Payment details not specified";
                return BadRequest(serverResponse);
            }
            foreach (var item in requestDto.payables)
            {
                var payable = payableDtos.data.Where(x => x.payableID == item.payableID && x.payableType == x.payableType).FirstOrDefault();
                if (payable == null) // Payment was already paid
                {
                    serverResponse.Success = false;
                    item.balance = 0;
                }
                else if (payable != null && item.paymentAmount > payable.balance)
                {
                    serverResponse.Success = false;
                    item.balance = payable.balance;
                }
            }
            if (await _paymentService.IsDuplicateReceipt(requestDto.receipttype, requestDto.orno))
            {
                serverResponse.Success = false;
                serverResponse.message = "Duplicated receipt number";
                return BadRequest(serverResponse);
            }
            if (!serverResponse.Success)
                return BadRequest(serverResponse);

            var result = await _paymentService.Create(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }
        [HttpPut("voidPayment")]
        public async Task<IActionResult> VoidPayment(PaymentRequestDto requestDto)
        {
            await _paymentService.VoidPayment(requestDto.paymentid, requestDto.voidremarks);
            return Ok(new ServerResponse<bool>());

        }
        [HttpGet("getreceiptno")]
        public async Task<IActionResult> GetReceiptNo(ReceiptType receiptType)
        {
            var serverResponse = new ServerResponse<long>
            {
                Data = await _paymentService.GetReceiptNo(receiptType)
            };
            return Ok(serverResponse);

        }
        [HttpGet("getcreditnotes")]
        public async Task<IActionResult> GetCreditNotes([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<PaymentViewModel>>
            {
                Data = await _paymentService.GetCreditNotes(request)
            };
            return Ok(serverResponse);

        }
    }
}
