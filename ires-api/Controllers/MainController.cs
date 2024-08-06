using AutoMapper;
using ires.AppService.Common;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController(
        IAppService _appService,
        IMapper _mapper,
        ISurveyService _surveyService,
        IClientService _clientService,
        IPaymentService _paymentService,
        IExpenseService _expenseService,
        IPettyCashService _pettyCashService,
        IRentalService _rentalService,
        ILogService _logService) : ControllerBase
    {


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
        public async Task<IActionResult> GetSurveyDashboard(DateTime currentDate)
        {
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var totalPayment = (await _paymentService.GetPayments(identity.companyid ?? 0, "", currentDate, currentDate)).Sum(x => x.totalamount);
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
        public async Task<IActionResult> GetRentalDashboard(DateTime currentDate)
        {
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var totalPayment = (await _paymentService.GetPayments(identity.companyid ?? 0, "", currentDate, currentDate)).Sum(x => x.totalamount);
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
        [HttpPost("markallasreadnotifs")]
        public async Task<IActionResult> MarkAllAsReadNotif(IDRequestDto requestDto)
        {
            await _appService.MarkAllAsReadNotif(requestDto.id);
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

        [HttpPost("savelog")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveLog([FromBody] LogRequestDto requestDto)
        {
            await _logService.SaveLogAsync(requestDto.companyid, requestDto.employeeid, requestDto.moduleid, requestDto.logtitle, requestDto.logAction, 0);
            return Ok(new ServerResponse<bool> { Message = "Log successfully saved!" });
        }

        [HttpPost("runjob/{job}")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveLog(string job)
        {
            var result = await _appService.ExecuteJob(job);
            return Ok(new ServerResponse<bool> { Success = result });
        }
    }
}
