using AutoMapper;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Survey;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Jobs.Billing;
using ires.Infrastructure.Jobs.RentalContract;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace ires.Infrastructure.Repositories
{
    public class AppRepository : IAppService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        private readonly ISchedulerFactory _schedulerFactory;

        public AppRepository(DataContext dataContext, IMapper mapper, ILogService logService, ISchedulerFactory schedulerFactory)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logService = logService;
            _schedulerFactory = schedulerFactory;
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

        public async Task<bool> ExecuteJob(string job)
        {
            JobKey jobKey = null;
            if (job == "bill")
                jobKey = JobKey.Create(nameof(BillingGenerationJob));
            else if (job == "rental")
                jobKey = JobKey.Create(nameof(RentalContractComputationJob));

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
