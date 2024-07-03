using ires.Domain.Contracts;
using Quartz;

namespace ires.Infrastructure.Jobs.Billing
{
    public class BillingGenerationJob : IJob
    {
        private readonly IBillService _billService;
        private readonly IAppService _appService;

        public BillingGenerationJob(IBillService billService, IAppService appService)
        {
            _billService = billService;
            _appService = appService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _appService.ReloadSubscriptions();
            var bills = await _billService.GetUnsentBills();
            foreach (var bill in bills)
            {
                await _billService.SendBill(bill.id);
            }
        }
    }
}
