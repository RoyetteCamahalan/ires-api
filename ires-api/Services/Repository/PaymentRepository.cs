using ires_api.Data;
using ires_api.DTO;
using ires_api.Services.Interface;

namespace ires_api.Services.Repository
{
    public class PaymentRepository : IPaymentService
    {
        private readonly DataContext _dataContext;

        public PaymentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public ICollection<PayableDto> GetPayables(long clientID, string search)
        {
            List<PayableDto> payables = new List<PayableDto>();
            payables = _dataContext.surveys.Where(x => x.custid == clientID && x.balance > 0 && x.propertyname.Contains(search)).Select(x => new PayableDto
            {
                payableType = Constants.AppModules.survey,
                payableID = x.id,
                description = "Survey Fee - " + x.propertyname + " (" + (x.surveydate ?? DateTime.Now).ToString(Constants.dateFormat) + ")",
                grossAmount = x.contractprice,
                balance = x.balance
            }).ToList();

            return payables;
        }
    }
}
