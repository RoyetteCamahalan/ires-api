using AutoMapper;
using ires.Domain.Contracts;
using ires.Domain.DTO.OtherCharge;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.SqlServer;

namespace ires.Infrastructure.Repositories
{
    public class ChargeRepository : IChargeService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ChargeRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<OtherChargeViewModel> Create(OtherChargeRequestDto requestDto)
        {
            var entity = _mapper.Map<OtherCharge>(requestDto);
            entity.chargeid = 0;
            entity.survey = null;
            _dataContext.otherCharges.Add(entity);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<OtherChargeViewModel>(entity);
        }

        private async Task<OtherCharge> GetOtherChargeByIDAsync(long id)
        {
            return await _dataContext.otherCharges.FindAsync(id);
        }

        public async Task<OtherChargeViewModel> GetOtherChargeByID(long id)
        {
            var result = await GetOtherChargeByIDAsync(id);
            return _mapper.Map<OtherChargeViewModel>(result);
        }

        public async Task<ICollection<OtherChargeViewModel>> GetOtherCharges(long surveyID, string search)
        {
            var result = await _dataContext.otherCharges.Include(x => x.fee).Where(x => x.surveyid == surveyID && x.surveyid > 0
                && (x.fee!.name.Contains(search) || SqlFunctions.StringConvert(x.chargeamount).Contains(search))).ToListAsync();
            return _mapper.Map<ICollection<OtherChargeViewModel>>(result);
        }

        public async Task<ICollection<OtherChargeViewModel>> GetOtherCharges(long invoiceNo, long lotID, string search)
        {
            var result = await _dataContext.otherCharges.Where(x => x.invoiceno == invoiceNo && x.lotid == lotID && x.invoiceno > 0
                && (x.fee!.name.Contains(search) || SqlFunctions.StringConvert(x.chargeamount).Contains(search))).ToListAsync();
            return _mapper.Map<ICollection<OtherChargeViewModel>>(result);
        }

        public async Task<bool> Update(OtherChargeRequestDto requestDto)
        {
            var otherCharge = await GetOtherChargeByIDAsync(requestDto.chargeid);
            if (otherCharge != null)
            {
                otherCharge.chargedate = requestDto.chargedate;
                otherCharge.chargeamount = requestDto.chargeamount;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
