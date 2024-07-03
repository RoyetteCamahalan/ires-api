using ires.Domain.Contracts;
using Quartz;

namespace ires.Infrastructure.Jobs.RentalContract
{
    [DisallowConcurrentExecution]
    public class RentalContractComputationJob : IJob
    {
        private readonly IAppService _appService;

        public RentalContractComputationJob(IAppService appService)
        {
            _appService = appService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _appService.ReloadRentalContracts();
        }
    }
}
