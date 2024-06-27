using ires.Domain.DTO;
using ires.Domain.DTO.Company;

namespace ires.Domain.Contracts
{
    public interface IBillService
    {
        public Task<ICollection<BillViewModel>> GetBills(int companyID, int filter);
        public Task<BillViewModel> GetBillByID(long billID);
        public Task<SubscriptionPlanViewModel> GetPlanByID(long planID);
        public Task<CompanyPlanViewModel> GetSubscriptionPlans(int companyID);
        public Task<bool> UpdateBillingCycle(RegisterCompanyRequestDto requestDto);
        public Task<BillViewModel> StartPayment(int companyID, long billID, PayMongoConfig payMongoConfig);
        public Task<BillViewModel> CompletePayment(int companyID, long billID, PayMongoConfig payMongoConfig);
        public Task<bool> UpgradePlan(int companyID, int planID, long employeeid);
    }
}
