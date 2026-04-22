using AutoMapper;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Survey;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using ires.Infrastructure.Jobs.Billing;
using ires.Infrastructure.Jobs.RentalContract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace ires.Infrastructure.Repositories
{
    public class AppRepository : IAppService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public AppRepository(DataContext dataContext, IMapper mapper, ILogService logService, ISchedulerFactory schedulerFactory, IMailService mailService, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logService = logService;
            _schedulerFactory = schedulerFactory;
            _mailService = mailService;
            _configuration = configuration;
        }

        public async Task<ICollection<NotificationViewModel>> GetNotifications(long employeeID)
        {
            var result = await _dataContext.notifications.Where(x => x.employeeid == employeeID && !x.isread).OrderByDescending(x => x.datecreated).ToListAsync();
            return _mapper.Map<ICollection<NotificationViewModel>>(result);
        }

        public async Task MarkAsReadNotif(long id)
        {
            var notif = await _dataContext.notifications.FindAsync(id);
            if (notif != null && !notif.isread)
            {
                notif.isread = true;
                await _dataContext.SaveChangesAsync();
            }
        }
        public async Task MarkAllAsReadNotif(long employeeID)
        {
            var notifs = await _dataContext.notifications.Where(x => x.employeeid == employeeID && !x.isread).ToListAsync();
            notifs.ForEach(x => x.isread = true);
            await _dataContext.SaveChangesAsync();
        }
        public async Task<ICollection<EventViewModel>> GetEvents(int companyID, DateTime startDate, DateTime endDate)
        {
            return await _dataContext.surveys.Where(x => x.companyid == companyID && x.status != SurveyStatus.cancelled &&
                (x.surveydate ?? DateTime.MaxValue) >= startDate && (x.surveydate ?? DateTime.MaxValue) <= endDate)
                .Select(x => new EventViewModel
                {
                    id = x.id,
                    moduleid = AppModule.Surveying,
                    title = "Survey at " + x.propertyname,
                    date = x.surveydate ?? DateTime.MaxValue,
                    survey = _mapper.Map<SurveyViewModel>(x),
                }).ToListAsync();
        }

        public async Task ReloadRentalContracts()
        {
            await _logService.SaveLogAsync(0, 0, 0, "CRON Job Rental", "Job Started", 1);
            try
            {
                await _dataContext.Database.ExecuteSqlRawAsync("exec spCronJobs @operation = 2, @soperation = 0");
                await _logService.SaveLogAsync(0, 0, 0, "CRON Job Rental", "Job Completed Successfully", 1);
            }
            catch (Exception ex)
            {
                await _logService.SaveLogAsync(0, 0, 0, "CRON Job Rental", "Error encountered: " + ex.Message, 1);
            }
        }

        public async Task ReloadSubscriptions()
        {
            await _logService.SaveLogAsync(0, 0, 0, "CRON Job Subscription", "Job Started", 1);
            try
            {
                await _dataContext.Database.ExecuteSqlRawAsync("exec spCronJobs @operation = 2, @soperation = 1");
                await _logService.SaveLogAsync(0, 0, 0, "CRON Job Subscription", "Job Completed Successfully", 1);
            }
            catch (Exception ex)
            {
                await _logService.SaveLogAsync(0, 0, 0, "CRON Job Subscription", "Error encountered: " + ex.Message, 1);
            }
        }

        public async Task SendBillingAccountNotifications()
        {
            await _logService.SaveLogAsync(0, 0, 0, "CRON Job Billing Notification", "Job Started", 1);
            try
            {
                var today = DateTime.UtcNow.Date;
                var twoDaysAgo = DateTime.UtcNow.AddDays(-2);

                // IgnoreQueryFilters so the job processes all companies, not just the ambient one
                var accounts = await _dataContext.billingAccounts
                    .IgnoreQueryFilters()
                    .Where(x =>
                        x.IsActive &&
                        x.NotifyOption != BillingNotification.None &&
                        x.NextDueDate != null &&
                        x.NextDueDate.Value.AddDays(-x.NotifyDaysBefore).Date <= today &&
                        (x.LastNotified == null || x.LastNotified.Value < twoDaysAgo))
                    .ToListAsync();

                if (accounts == null || accounts.Count == 0) return;

                foreach (var account in accounts)
                {
                    var recipientEmployees = new List<Employee>();

                    if (account.NotifyOption == BillingNotification.AllUsers)
                    {
                        var employees = await _dataContext.employees
                            .Where(e => e.companyid == account.CompanyId && e.isactive)
                            .ToListAsync();
                        recipientEmployees.AddRange(employees);
                    }
                    else if (account.NotifyOption == BillingNotification.OnlyMe)
                    {
                        var employee = await _dataContext.employees
                            .FirstOrDefaultAsync(e => e.employeeid == account.CreatedById && e.isactive);
                        if (employee != null)
                            recipientEmployees.Add(employee);
                    }

                    if (recipientEmployees.Count == 0) continue;

                    var dueDate = account.NextDueDate!.Value.ToString("MMM dd, yyyy");
                    var details = $"Billing account '{account.AccountName}' is due on {dueDate}.";

                    var notifications = recipientEmployees.Select(e => new Notification
                    {
                        employeeid = e.employeeid,
                        details = details,
                        url = $"/billing-accounts/{account.Id}",
                        datecreated = DateTime.UtcNow,
                        isread = false,
                        typeid = (int)AppModule.Billing
                    }).ToList();

                    _dataContext.notifications.AddRange(notifications);
                    account.LastNotified = DateTime.UtcNow;

                    // Send email reminders
                    var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "BillingAccountDueReminder.html");
                    if (File.Exists(templatePath))
                    {
                        var htmlTemplate = File.ReadAllText(templatePath);
                        foreach (var employee in recipientEmployees)
                        {
                            if (string.IsNullOrWhiteSpace(employee.email)) continue;
                            var body = htmlTemplate
                                .Replace("{base_url}", _configuration["uiBaseURL"])
                                .Replace("{user_firstname}", employee.firstname)
                                .Replace("{account_name}", account.AccountName)
                                .Replace("{due_date}", dueDate);
                            _mailService.SendEmailAsync("Billing Due Reminder: " + account.AccountName, new List<string> { employee.email }, body, true);
                        }
                    }
                }

                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(0, 0, 0, "CRON Job Billing Notification", "Job Completed Successfully", 1);
            }
            catch (Exception ex)
            {
                await _logService.SaveLogAsync(0, 0, 0, "CRON Job Billing Notification", "Error encountered: " + ex.Message, 1);
            }
        }

        public async Task<bool> ExecuteJob(string job)
        {
            JobKey jobKey = null;
            if (job == "bill")
                jobKey = JobKey.Create(nameof(BillingGenerationJob));
            else if (job == "rental")
                jobKey = JobKey.Create(nameof(RentalContractComputationJob));
            else if (job == "billing-notification")
                jobKey = JobKey.Create(nameof(BillingNotificationJob));

            if (jobKey != null)
            {
                var scheduler = await _schedulerFactory.GetScheduler();
                await scheduler.TriggerJob(jobKey);
                return true;
            }
            return false;
        }
    }
}
