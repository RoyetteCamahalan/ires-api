using Microsoft.Extensions.Options;
using Quartz;

namespace ires.Infrastructure.Jobs.Billing
{
    public class BillingNotificationJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(BillingNotificationJob));
            options
                .AddJob<BillingNotificationJob>(builder => builder.WithIdentity(jobKey))
                .AddTrigger(trigger =>
                    trigger.ForJob(jobKey).WithCronSchedule("0 0 23 * * ?")); // At 11:00 PM PST every day = 7:00 AM Manila
        }
    }
}
