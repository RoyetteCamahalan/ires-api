using AutoMapper;
using ires.AppService.Common;
using ires.Domain;
using ires.Domain.Common;
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
            ;
            var totalExpenses = (await _expenseService.GetExpenses(
                new PaginationRequest { startDate = Utility.GetServerTime(), endDate = Utility.GetServerTime() }))
                .data.Where(x => x.status == ExpenseStatus.approved).Select(x => x.amount).Sum();
            var data = new
            {
                totalExpenses,
                totalPettyCash = await _pettyCashService.TotalPettyCashBalance(),
                totalVendors = await _expenseService.CountVendors()
            };
            var serverResponse = new ServerResponse<Object> { Data = data };
            return Ok(serverResponse);
        }
        [HttpGet("getsurveydashboard")]
        public async Task<IActionResult> GetSurveyDashboard(DateTime currentDate)
        {
            var totalPayment = (await _paymentService.GetPayments(
                new PaginationRequest { startDate = currentDate, endDate = currentDate })).data.Sum(x => x.totalamount);
            var data = new
            {
                totalPayment,
                totalSurveys = await _surveyService.CountCompleted(),
                pendingSurveys = await _surveyService.CountPending(),
                totalClients = await _clientService.GetCountClientAsync(),
            };
            var serverResponse = new ServerResponse<Object> { Data = data };
            return Ok(serverResponse);
        }
        [HttpGet("getrentaldashboard")]
        public async Task<IActionResult> GetRentalDashboard(DateTime currentDate)
        {
            var totalPayment = (await _paymentService.GetPayments(
                new PaginationRequest { startDate = currentDate, endDate = currentDate })).data.Sum(x => x.totalamount);
            var data = new
            {
                totalPayment,
                activeContracts = await _rentalService.CountActiveContracts(),
                availableProperties = await _rentalService.CountAvailableUnits(),
                totalProperties = await _rentalService.CountActiveUnits(),
            };
            var serverResponse = new ServerResponse<Object> { Data = data };
            return Ok(serverResponse);
        }
        [HttpGet("getnotifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var result = await _appService.GetNotifications();
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
        public async Task<IActionResult> MarkAllAsReadNotif()
        {
            await _appService.MarkAllAsReadNotif();
            return Ok(new ServerResponse<bool>());
        }

        [HttpGet("getcalendarevents")]
        public async Task<IActionResult> GetCalendarEvents(DateTime startDate, DateTime endDate)
        {
            var serverResponse = new ServerResponse<ICollection<EventViewModel>>
            {
                Data = await _appService.GetEvents(startDate.Date, endDate.Date)
            };
            return Ok(serverResponse);
        }

        [HttpPost("savelog")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveLog([FromBody] LogRequestDto requestDto)
        {
            await _logService.SaveLogAsync(requestDto.companyid, requestDto.employeeid, requestDto.moduleid, requestDto.logtitle, requestDto.logAction, 0);
            return Ok(new ServerResponse<bool> { message = "Log successfully saved!" });
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
