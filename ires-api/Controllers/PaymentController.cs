using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Payment;
using ires.Domain.Enumerations;
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

        [HttpGet]
        public async Task<IActionResult> Get(int currentPage, DateTime startDate, DateTime endDate, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<PaymentViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var payments = await _paymentService.GetPayments(identity.companyid ?? 0, search ?? "", startDate, endDate);
            var paginator = new PaginatorDto<PaymentViewModel>(currentPage);
            paginator.Paginate(payments);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("getcollection")]
        public async Task<IActionResult> GetCollection(DateTime startDate, DateTime endDate)
        {
            var serverResponse = new ServerResponse<PaymentCollectionViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var payments = await _paymentService.GetPayments(identity.companyid ?? 0, "", startDate, endDate);
            payments = payments.Where(x => x.status != PaymentStatus.@void).ToList();
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
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var payment = await _paymentService.GetPayment(id);
            payment.payables = await _paymentService.GetPaymentDetailsAsPayables(payment.paymentid);
            serverResponse.Data = payment;
            return Ok(serverResponse);

        }
        [HttpGet("getpayables")]
        public async Task<IActionResult> GetPayables(long clientID, int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<PayableViewModel>>();
            var payableDtos = await _paymentService.GetPayables(clientID, search ?? "");
            var paginator = new PaginatorDto<PayableViewModel>(currentPage);
            paginator.Paginate(payableDtos);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<PaymentViewModel>();
            var payableDtos = await _paymentService.GetPayables(requestDto.custid, "");
            if (requestDto.payables.Count == 0)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Payment details not specified";
                return BadRequest(serverResponse);
            }
            foreach (var item in requestDto.payables)
            {
                var payable = payableDtos.Where(x => x.payableID == item.payableID && x.payableType == x.payableType).FirstOrDefault();
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
            if (await _paymentService.IsDuplicateReceipt(requestDto.companyid, requestDto.receipttype, requestDto.orno))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Duplicated receipt number";
                return BadRequest(serverResponse);
            }
            if (!serverResponse.Success)
                return BadRequest(serverResponse);

            var result = await _paymentService.Create(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }
        [HttpPut("voidPayment")]
        public async Task<IActionResult> VoidPayment(PaymentRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            serverResponse.Success = await _paymentService.VoidPayment(requestDto.paymentid, identity.employeeid, requestDto.voidremarks);
            serverResponse.Data = serverResponse.Success;
            if (!serverResponse.Success)
            {
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);

        }
        [HttpGet("getreceiptno")]
        public async Task<IActionResult> GetReceiptNo(ReceiptType receiptType)
        {
            var serverResponse = new ServerResponse<long>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var receiptNo = await _paymentService.GetReceiptNo(identity.companyid ?? 0, receiptType);
            serverResponse.Data = receiptNo;
            return Ok(serverResponse);

        }
        [HttpGet("getcreditnotes")]
        public async Task<IActionResult> GetCreditNotes(int currentPage, DateTime startDate, DateTime endDate, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<PaymentViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var payments = await _paymentService.GetCreditNotes(identity.companyid ?? 0, search ?? "", startDate, endDate);
            var paginator = new PaginatorDto<PaymentViewModel>(currentPage);
            paginator.Paginate(payments);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
    }
}
