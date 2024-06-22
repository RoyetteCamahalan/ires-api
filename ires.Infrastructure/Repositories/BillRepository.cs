using AutoMapper;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Company;
using ires.Domain.DTO.Payment;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace ires.Infrastructure.Repositories
{
    public class BillRepository : IBillService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public BillRepository(DataContext dataContext, IMapper mapper, ILogService logService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logService = logService;
        }

        public async Task<BillViewModel> GetBillByID(long billID)
        {
            var result = await GetBillByIDAsync(billID);
            return _mapper.Map<BillViewModel>(result);
        }
        private async Task<Bill> GetBillByIDAsync(long billID)
        {
            return await _dataContext.bills.Include(x => x.company).FirstOrDefaultAsync(x => x.id == billID);
        }

        public async Task<ICollection<BillViewModel>> GetBills(int companyID, int filter)
        {
            ICollection<Bill> result;
            if (filter == 0)
                result = await _dataContext.bills.Where(x => x.companyid == companyID && x.status == BillStatus.open).OrderBy(x => x.billdate).ToListAsync();
            else if (filter == 1)
                result = await _dataContext.bills.Where(x => x.companyid == companyID && x.status != BillStatus.open).OrderBy(x => x.billdate).ToListAsync();
            else
                result = await _dataContext.bills.Where(x => x.companyid == companyID).OrderBy(x => x.billdate).ToListAsync();
            return _mapper.Map<ICollection<BillViewModel>>(result);
        }

        public async Task<CompanyPlanViewModel> GetSubscriptionPlans(int companyID)
        {
            var result = await _dataContext.companies.Include(x => x.subscriptionPlan).Where(x => x.id == companyID).FirstOrDefaultAsync();
            return _mapper.Map<CompanyPlanViewModel>(result);
        }

        public async Task<BillViewModel> StartPayment(int companyID, long billID, PayMongoConfig payMongoConfig)
        {
            var client = new RestClient(payMongoConfig.apiURL);
            var request = new RestRequest
            {
                Method = Method.Post
            };
            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(payMongoConfig.secretKey)));
            var bill = await GetBillByIDAsync(billID);
            if (bill.companyid != companyID)
                return null;
            PayMongoRequestDto requestDto = new PayMongoRequestDto(_mapper.Map<BillViewModel>(bill), payMongoConfig);
            request.AddParameter("application/json", JsonConvert.SerializeObject(new { data = requestDto }), ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
            {
                PayMongoResponseDto responseDto = JsonConvert.DeserializeObject<PayMongoResponseDto>(response.Content);
                bill.checkouturl = responseDto.data.attributes.checkout_url;
                bill.paymentid = responseDto.data.id;
            }
            else
                bill.checkouturl = "";
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<BillViewModel>(bill);
        }

        public async Task<BillViewModel> CompletePayment(int companyID, long billID, PayMongoConfig payMongoConfig)
        {
            var bill = await GetBillByIDAsync(billID);
            if (bill != null && bill.paymentid.Length > 0)
            {
                var client = new RestClient(payMongoConfig.apiURL + '/' + bill.paymentid);
                var request = new RestRequest
                {
                    Method = Method.Get
                };
                request.AddHeader("accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(payMongoConfig.secretKey)));
                RestResponse response = await client.ExecuteAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
                {
                    PayMongoResponseDto responseDto = JsonConvert.DeserializeObject<PayMongoResponseDto>(response.Content);
                    if (responseDto.data.attributes.payments.Count > 0 && responseDto.data.attributes.payments[0].attributes.status == "paid")
                    {
                        bill.paymentrefno = responseDto.data.attributes.payments[0].id;
                        bill.paymentmode = responseDto.data.attributes.payment_method_used;
                        bill.status = BillStatus.paid;
                        bill.datepaid = DateTimeOffset.FromUnixTimeSeconds(responseDto.data.attributes.payments[0].attributes.paid_at).DateTime;
                        if (!_dataContext.bills.Where(x => x.companyid == companyID && x.id != billID && x.status == BillStatus.open).Any())
                        {
                            Company company = _dataContext.companies.Find(companyID);
                            company.subscriptionexpiry = bill.dateend ?? DateTime.Now;
                        }
                        await _dataContext.SaveChangesAsync();
                    }
                }
            }
            return _mapper.Map<BillViewModel>(bill);
        }

        public async Task<bool> UpdateBillingCycle(CompanyRequestDto requestDto)
        {
            var entity = await _dataContext.companies.FindAsync(requestDto.id);
            if (entity != null)
            {
                entity.billingcycle = requestDto.billingcycle;
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(entity.id, requestDto.updatedbyid, AppModule.Billing, "Billing Cycle", "Updated Billing Cycle : " + (requestDto.billingcycle == BillingCycle.yearly ? "Yearly" : "Monthly"), 1);
                return true;
            }
            return false;
        }

        public async Task<bool> UpgradePlan(int companyID, int planID, long employeeid)
        {
            var company = _dataContext.companies.Find(companyID);

            var plan = await GetPlan(planID);
            if (company.amount == 0 && plan.monthlysubscription > 0)
            {
                company.subscriptionexpiry = DateTime.Now.AddDays(15);
                var bill = new Bill
                {
                    companyid = companyID,
                    billdate = DateTime.Now,
                    datefrom = DateTime.Now,
                    duedate = company.subscriptionexpiry,
                    status = BillStatus.open,
                };
                if (company.billingcycle == BillingCycle.monthly)
                {
                    bill.dateend = DateTime.Now.AddMonths(1);
                    bill.amount = plan.monthlysubscription;
                }
                else if (company.billingcycle == BillingCycle.yearly)
                {
                    bill.dateend = DateTime.Now.AddYears(1);
                    bill.amount = plan.monthlysubscription * 12;
                }
                bill.balance = bill.amount;
                bill.particular = "Subscription for " + (bill.datefrom ?? DateTime.Now).ToString("MMM dd, yyyy") + " - " + (bill.dateend ?? DateTime.Now).ToString("MMM dd, yyyy");
                _dataContext.bills.Add(bill);
            }
            company.planid = planID;
            company.amount = plan.monthlysubscription;
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(company.id, employeeid, AppModule.Billing, "Subscription Upgrade", "Subscription Upgrade : " + planID, 1);
            return true;
        }

        private async Task<SubscriptionPlan> GetPlan(long planID)
        {
            return await _dataContext.subscriptionPlans.Where(x => x.id == planID).FirstOrDefaultAsync();
        }

        public async Task<SubscriptionPlanViewModel> GetPlanByID(long planID)
        {
            var result = await GetPlan(planID);
            return _mapper.Map<SubscriptionPlanViewModel>(result);
        }
    }
}
