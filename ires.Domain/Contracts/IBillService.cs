using ires.Domain.Common;
using ires.Domain.DTO;
using ires.Domain.DTO.Company;

namespace ires.Domain.Contracts
{
    public interface IBillService
    {
        public Task<PaginatedResult<BillViewModel>> GetBills(PaginationRequest paginationRequest);
        public Task<BillViewModel> GetBillByID(long billID);
        public Task<SubscriptionPlanViewModel> GetPlanByID(long planID);
        public Task<CompanyPlanViewModel> GetSubscriptionPlans();
        public Task UpdateBillingCycle(RegisterCompanyRequestDto requestDto);
        public Task<BillViewModel> StartPayment(long billID, PayMongoConfig payMongoConfig);
        public Task<BillViewModel> CompletePayment(long billID, PayMongoConfig payMongoConfig);
        public Task UpgradePlan(int planID);

        public Task<ICollection<BillViewModel>> GetUnsentBills();
        public Task<FileDataViewModel> GenerateInvoice(long id);
        public Task SendBill(long id);
    }
}
