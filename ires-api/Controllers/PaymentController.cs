using AutoMapper;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ISurveyService _surveyService;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService paymentService, ISurveyService surveyService, IMapper mapper)
        {
            _paymentService = paymentService;
            _surveyService = surveyService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int currentPage, DateTime startDate, DateTime endDate, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<PaymentDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var payments = await _paymentService.GetPayments(identity.companyid ?? 0, search ?? "", startDate, endDate);
            var paginator = new PaginatorDto<PaymentDto>(currentPage);
            paginator.Paginate(_mapper.Map<List<PaymentDto>>(payments));
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<PaymentDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var payment = await _paymentService.GetPayment(id);
            var paymentDto = _mapper.Map<PaymentDto>(payment);
            paymentDto.payables = new List<PayableDto>();
            foreach (var item in payment.paymentDetails)
            {
                var detail = new PayableDto
                {
                    paymentAmount = item.amount,
                    payableType = item.payableType,
                };
                if (detail.payableType == Constants.AppModules.survey)
                {
                    var survey = await _surveyService.GetSurveyByID(item.surveyid);
                    detail.payableID = item.surveyid;
                    detail.description = "Survey Fee - " + survey.propertyname + " (" + (survey.surveydate ?? DateTime.Now).ToString(Constants.dateFormat) + ")";
                }
                paymentDto.payables.Add(detail);
            }
            serverResponse.Data = paymentDto;
            return Ok(serverResponse);

        }
        [HttpGet("getpayables")]
        public async Task<IActionResult> GetPayables(long clientID, int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<PayableDto>>();
            var payableDtos = await _paymentService.GetPayables(clientID, search ?? "");
            var paginator = new PaginatorDto<PayableDto>(currentPage);
            paginator.Paginate(payableDtos);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<PaymentRequestDto>();
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
            {
                serverResponse.Data = requestDto;
                return BadRequest(serverResponse);
            }

            var result = await _paymentService.Create(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<PaymentRequestDto>(result);
            return Ok(serverResponse);
        }
        [HttpPut("voidPayment")]
        public async Task<IActionResult> VoidPayment(PaymentRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            serverResponse.Success = await _paymentService.VoidPayment(requestDto.paymentid);
            serverResponse.Data = serverResponse.Success;
            if (!serverResponse.Success)
            {
                serverResponse.Message = "Payment not found";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);

        }
        [HttpGet("getreceiptno")]
        public async Task<IActionResult> GetReceiptNo(int receiptType)
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
    }
}
