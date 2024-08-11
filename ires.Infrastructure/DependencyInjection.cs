using ires.Infrastructure.Jobs.Billing;
using ires.Infrastructure.Jobs.RentalContract;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace ires.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });
            services.ConfigureOptions<RentalContractComputationJobSetup>();
            services.ConfigureOptions<BillingGenerationJobSetup>();
        }
    }
}
