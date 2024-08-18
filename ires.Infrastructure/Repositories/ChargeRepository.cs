using AutoMapper;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.OtherCharge;
using ires.Domain.Exceptions;
using ires.Infrastructure.Extensions;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.SqlServer;

namespace ires.Infrastructure.Repositories
{
    public class ChargeRepository(
        DataContext _dataContext,
        IMapper _mapper) : IChargeService
    {

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
            return await _dataContext.otherCharges.FindAsync(id) ?? throw new EntityNotFoundException();
        }

        public async Task<OtherChargeViewModel> GetOtherChargeByID(long id)
        {
            var result = await GetOtherChargeByIDAsync(id);
            return _mapper.Map<OtherChargeViewModel>(result);
        }

        public async Task<PaginatedResult<OtherChargeViewModel>> GetOtherCharges(long surveyID, PaginationRequest request)
        {
            var query = _dataContext.otherCharges.Include(x => x.fee).Where(x => x.surveyid == surveyID && x.surveyid > 0
                && (x.fee!.name.Contains(request.searchString) || SqlFunctions.StringConvert(x.chargeamount).Contains(request.searchString))).AsQueryable();
            return await query.AsPaginatedResult<OtherCharge, OtherChargeViewModel>(request, _mapper.ConfigurationProvider);
        }

        public async Task<ICollection<OtherChargeViewModel>> GetOtherCharges(long invoiceNo, long lotID, string search)
        {
            var result = await _dataContext.otherCharges.Where(x => x.invoiceno == invoiceNo && x.lotid == lotID && x.invoiceno > 0
                && (x.fee!.name.Contains(search) || SqlFunctions.StringConvert(x.chargeamount).Contains(search))).ToListAsync();
            return _mapper.Map<ICollection<OtherChargeViewModel>>(result);
        }

        public async Task Update(OtherChargeRequestDto requestDto)
        {
            var otherCharge = await GetOtherChargeByIDAsync(requestDto.chargeid);
            otherCharge.chargedate = requestDto.chargedate;
            otherCharge.chargeamount = requestDto.chargeamount;
            await _dataContext.SaveChangesAsync();
        }
    }
}
