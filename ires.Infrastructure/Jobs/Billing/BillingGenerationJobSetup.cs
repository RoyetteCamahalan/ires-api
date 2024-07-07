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
                    //trigger.ForJob(jobKey).WithSimpleSchedule(sched => sched.WithIntervalInSeconds(60).RepeatForever()));
                    trigger.ForJob(jobKey).WithCronSchedule("0 0 10 * * ?")); // At 10:00 AM PST every day) = 1:00AM IN Manila;
        }
    }
}
