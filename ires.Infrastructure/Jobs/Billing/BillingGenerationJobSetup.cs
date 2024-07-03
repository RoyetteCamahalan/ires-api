using Microsoft.Extensions.Options;
using Quartz;

namespace ires.Infrastructure.Jobs.Billing
{
    public class BillingGenerationJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(BillingGenerationJob));
            options
                .AddJob<BillingGenerationJob>(builder => builder.WithIdentity(jobKey))
                .AddTrigger(trigger =>
                    trigger.ForJob(jobKey).WithSimpleSchedule(sched => sched.WithIntervalInSeconds(60).RepeatForever()));
            //trigger.ForJob(jobKey).WithDailyTimeIntervalSchedule(sched => sched.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(1, 0)).OnEveryDay()));
        }
    }
}
