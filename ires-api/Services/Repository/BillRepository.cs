using ires_api.Data;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace ires_api.Services.Repository
{
    public class BillRepository : IBillService
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public BillRepository(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }

        public Bill GetBillByID(long billID)
        {
            return _dataContext.bills.Find(billID);
        }

        public ICollection<Bill> GetBills(int companyID, int filter)
        {
            if (filter == 0)
                return _dataContext.bills.Where(x => x.companyid == companyID && x.status == Constants.BillStatus.open).OrderBy(x => x.billdate).ToList();
            else if (filter == 1)
                return _dataContext.bills.Where(x => x.companyid == companyID && x.status != Constants.BillStatus.open).OrderBy(x => x.billdate).ToList();

            return _dataContext.bills.Where(x => x.companyid == companyID).OrderBy(x => x.billdate).ToList();
        }

        public ICollection<Company> GetSubscriptionPlans(int companyID)
        {
            return _dataContext.companies.Include(x => x.subscriptionPlan).Where(x => x.id == companyID).ToList();
        }

        public Bill StartPayment(int companyID, long billID)
        {
            var client = new RestClient(_configuration["PayMongo:apiURL"].ToString());
            var request = new RestRequest
            {
                Method = Method.Post
            };
            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(_configuration["PayMongo:secretKey"].ToString())));
            Bill bill = GetBillByID(billID);
            PayMongoRequestDto requestDto = new PayMongoRequestDto(bill, _configuration);
            request.AddParameter("application/json", JsonConvert.SerializeObject(new { data = requestDto }), ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                PayMongoResponseDto responseDto = JsonConvert.DeserializeObject<PayMongoResponseDto>(response.Content);
                bill.checkouturl = responseDto.data.attributes.checkout_url;
                bill.paymentid = responseDto.data.id;
            }
            else
            {
                bill.checkouturl = "";
            }
            _dataContext.SaveChanges();
            return bill;
        }

        public Bill CompletePayment(int companyID, long billID)
        {
            Bill bill = GetBillByID(billID);
            if (bill != null && bill.paymentid.Length > 0)
            {
                var client = new RestClient(_configuration["PayMongo:apiURL"].ToString() + '/' + bill.paymentid);
                var request = new RestRequest
                {
                    Method = Method.Get
                };
                request.AddHeader("accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(_configuration["PayMongo:secretKey"].ToString())));
                RestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
                {
                    PayMongoResponseDto responseDto = JsonConvert.DeserializeObject<PayMongoResponseDto>(response.Content);
                    if (responseDto.data.attributes.payments.Count > 0 && responseDto.data.attributes.payments[0].attributes.status == "paid")
                    {
                        bill.paymentrefno = responseDto.data.attributes.payments[0].id;
                        bill.paymentmode = responseDto.data.attributes.payment_method_used;
                        bill.status = Constants.BillStatus.paid;
                        if (_dataContext.bills.Where(x => x.companyid == companyID && x.id != billID && x.status == Constants.BillStatus.open).Count() == 0)
                        {
                            Company company = _dataContext.companies.Find(companyID);
                            company.subscriptionexpiry = (bill.dateend ?? DateTime.Now);
                        }
                        _dataContext.SaveChanges();
                    }
                }
            }
            return bill;
        }

        public void UpdateBillingCycle(int companyID, int cycleID)
        {
            var company = _dataContext.companies.Find(companyID);
            company.billingcycle = cycleID;
            _dataContext.SaveChanges();
        }

        public bool UpgradePlan(int companyID, int planID)
        {
            var company = _dataContext.companies.Find(companyID);
            var plan = GetPlanByID(planID);
            if (planID > company.planid && plan != null)
            {
                company.planid = planID;
                company.amount = plan.monthlysubscription;
                company.subscriptionexpiry = DateTime.Now.AddDays(30);
                _dataContext.SaveChanges();
                return true;
            }
            return false;
        }

        public SubscriptionPlan GetPlanByID(long planID)
        {
            return _dataContext.subscriptionPlans.Where(x => x.id == planID).FirstOrDefault();
        }
    }
}
