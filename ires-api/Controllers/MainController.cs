using AutoMapper;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IAppService _appService;
        private readonly IMapper _mapper;
        private readonly ISurveyService _surveyService;
        private readonly IClientService _clientService;
        private readonly IPaymentService _paymentService;
        private readonly IExpenseService _expenseService;
        private readonly IPettyCashService _pettyCashService;
        private readonly IRentalService _rentalService;

        public MainController(IAppService appService, IMapper mapper, ISurveyService surveyService, IClientService clientService, IPaymentService paymentService, IExpenseService expenseService,
            IPettyCashService pettyCashService, IRentalService rentalService)
        {
            _appService = appService;
            _mapper = mapper;
            _surveyService = surveyService;
            _clientService = clientService;
            _paymentService = paymentService;
            _expenseService = expenseService;
            _pettyCashService = pettyCashService;
            _rentalService = rentalService;
        }
        [HttpGet("getfinancedashboard")]
        public async Task<IActionResult> GetFinanceDashboard()
        {
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var totalExpenses = (await _expenseService.GetExpenses(identity.companyid ?? 0, "", Utility.GetServerTime(), Utility.GetServerTime())).Where(x => x.status == ExpenseStatus.approved).Select(x => x.amount).Sum();
            var data = new
            {
                totalExpenses,
                totalPettyCash = await _pettyCashService.TotalPettyCashBalance(identity.companyid ?? 0),
                totalVendors = await _expenseService.CountVendors(identity.companyid ?? 0)
            };
            var serverResponse = new ServerResponse<Object> { Data = data };
            return Ok(serverResponse);
        }
        [HttpGet("getsurveydashboard")]
        public async Task<IActionResult> GetSurveyDashboard()
        {
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var totalPayment = (await _paymentService.GetPayments(identity.companyid ?? 0, "", Utility.GetServerTime(), Utility.GetServerTime())).Select(x => x.totalamount).Sum();
            var data = new
            {
                totalPayment,
                totalSurveys = await _surveyService.CountCompleted(identity.companyid ?? 0),
                pendingSurveys = await _surveyService.CountPending(identity.companyid ?? 0),
                totalClients = await _clientService.GetCountClientAsync(identity.companyid ?? 0),
            };
            var serverResponse = new ServerResponse<Object> { Data = data };
            return Ok(serverResponse);
        }
        [HttpGet("getrentaldashboard")]
        public async Task<IActionResult> GetRentalDashboard()
        {
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var totalPayment = (await _paymentService.GetPayments(identity.companyid ?? 0, "", Utility.GetServerTime(), Utility.GetServerTime())).Select(x => x.totalamount).Sum();
            var data = new
            {
                totalPayment,
                activeContracts = await _rentalService.CountActiveContracts(identity.companyid ?? 0),
                availableProperties = await _rentalService.CountAvailableUnits(identity.companyid ?? 0),
                totalProperties = await _rentalService.CountActiveUnits(identity.companyid ?? 0),
            };
            var serverResponse = new ServerResponse<Object> { Data = data };
            return Ok(serverResponse);
        }
        [HttpGet("getnotifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = (await _appService.GetNotifications(identity.employeeid));
            var serverResponse = new ServerResponse<List<NotificationViewModel>>
            {
                Data = _mapper.Map<List<NotificationViewModel>>(result)
            };
            return Ok(serverResponse);
        }
        [HttpPut("readnotification")]
        public async Task<IActionResult> ReadNotification(IDRequestDto requestDto)
        {
            await _appService.MarkAsReadNotif(requestDto.id);
            return Ok(new ServerResponse<string>());
        }

        [HttpGet("getcalendarevents")]
        public async Task<IActionResult> GetCalendarEvents(DateTime startDate, DateTime endDate)
        {
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var serverResponse = new ServerResponse<ICollection<EventViewModel>>
            {
                Data = await _appService.GetEvents(identity.companyid ?? 0, startDate.Date, endDate.Date)
            };
            return Ok(serverResponse);
        }

    }
}
