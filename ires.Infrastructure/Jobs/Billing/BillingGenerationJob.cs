using ires.Domain.Contracts;
using Quartz;

namespace ires.Infrastructure.Jobs.Billing
{
    public class BillingGenerationJob(IBillService _billService, IAppService _appService) : IJob
    {

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
