using Microsoft.Extensions.Options;
using Quartz;

namespace ires.Infrastructure.Jobs.RentalContract
{
    public class RentalContractComputationJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(RentalContractComputationJob));
            options
                .AddJob<RentalContractComputationJob>(builder => builder.WithIdentity(jobKey))
                .AddTrigger(trigger =>
                    //trigger.ForJob(jobKey).WithSimpleSchedule(sched => sched.WithIntervalInSeconds(10).RepeatForever()));
                    trigger.ForJob(jobKey).WithCronSchedule("0 0 1 * * ?")); // At 1:00 AM every day);
        }
    }
}
