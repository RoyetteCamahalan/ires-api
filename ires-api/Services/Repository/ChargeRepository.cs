using ires_api.Data;
using ires_api.DTO.OtherCharge;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.SqlServer;

namespace ires_api.Services.Repository
{
    public class ChargeRepository : IChargeService
    {
        private readonly DataContext _dataContext;

        public ChargeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public OtherCharge Create(OtherCharge otherCharge)
        {
            otherCharge.chargeid = 0;
            otherCharge.survey = null;
            _dataContext.otherCharges.Add(otherCharge);
            return otherCharge;
        }

        public OtherCharge GetOtherChargeByID(long id)
        {
            return _dataContext.otherCharges.Find(id);
        }

        public ICollection<OtherCharge> GetOtherCharges(long surveyID, string search)
        {
            return _dataContext.otherCharges.Include(x => x.fee).Where(x => x.surveyid == surveyID && x.surveyid > 0 
                && (x.fee!.name.Contains(search) || SqlFunctions.StringConvert(x.chargeamount).Contains(search))).ToList();
        }

        public ICollection<OtherCharge> GetOtherCharges(long invoiceNo, long lotID, string search)
        {
            return _dataContext.otherCharges.Where(x => x.invoiceno == invoiceNo && x.lotid == lotID && x.invoiceno > 0
                && (x.fee!.name.Contains(search) || SqlFunctions.StringConvert(x.chargeamount).Contains(search))).ToList();
        }

        public OtherCharge Update(OtherChargeRequestDto requestDto)
        {
            OtherCharge otherCharge = GetOtherChargeByID(requestDto.chargeid);
            if(otherCharge != null)
            {
                otherCharge.chargedate = requestDto.chargedate;
                otherCharge.chargeamount = requestDto.chargeamount;
            }
            return otherCharge;
        }
    }
}
