using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IBillService
    {
        public ICollection<Bill> GetBills(int companyID, int filter);
        public Bill GetBillByID(long billID);
        public SubscriptionPlan GetPlanByID(long planID);
        public ICollection<Company> GetSubscriptionPlans(int companyID);
        public void UpdateBillingCycle(int companyID, int cycleID);
        public Bill StartPayment(int companyID, long billID);
        public Bill CompletePayment(int companyID, long billID);
        public bool UpgradePlan(int companyID, int planID);
    }
}
