using ires.Domain.Contracts;
using Quartz;

namespace ires.Infrastructure.Jobs.Billing
{
    [DisallowConcurrentExecution]
    public class BillingNotificationJob : IJob
    {
        private readonly IAppService _appService;

        public BillingNotificationJob(IAppService appService)
        {
            _appService = appService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _appService.SendBillingAccountNotifications();
        }
    }
}
