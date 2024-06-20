using AutoMapper;
using ires.Domain.Contracts;
using ires.Domain.DTO.OtherFee;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class OtherChargeRepository : IOtherChargeService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public OtherChargeRepository(DataContext dataContext, IMapper mapper, ILogService logService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logService = logService;
        }
        public async Task<OtherFeeViewModel> CreateOtherFee(OtherFeeRequestDto requestDto)
        {
            var entity = _mapper.Map<OtherFee>(requestDto);
            entity.id = 0;
            entity.datecreated = DateTime.Now;
            _dataContext.otherFees.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(entity.companyid, entity.createdby, AppModule.OtherFees, "Create Other Fee", "Create New Record : " + entity.id.ToString() + " - " + entity.name, 0);
            return _mapper.Map<OtherFeeViewModel>(entity);
        }
        private async Task<OtherFee> GetOtherFeeById(long id)
        {
            return await _dataContext.otherFees.FindAsync(id);
        }

        public async Task<OtherFeeViewModel> GetOtherFee(long id)
        {
            return _mapper.Map<OtherFeeViewModel>(await GetOtherFeeById(id));
        }

        public async Task<ICollection<OtherFeeViewModel>> GetOtherFees(int companyID, string search, bool viewAll)
        {
            var result = await _dataContext.otherFees.Where(x => x.companyid == companyID && (x.isactive || viewAll) && x.name.Contains(search))
                .OrderBy(x => x.name).ToListAsync();
            return _mapper.Map<ICollection<OtherFeeViewModel>>(result);
        }

        public async Task<bool> UpdateOtherFee(OtherFeeRequestDto requestDto)
        {
            var entity = await GetOtherFeeById(requestDto.id);
            if (entity != null)
            {
                entity.name = requestDto.name;
                entity.price = requestDto.price ?? 0;
                entity.description = requestDto.description;
                entity.isactive = requestDto.isactive;
                entity.updatedbyid = requestDto.updatedbyid;
                entity.dateupdated = DateTime.Now;
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(entity.companyid, entity.updatedbyid ?? 0, AppModule.OtherFees, "Update Other Fee", "Update Record : " + requestDto.id.ToString() + " - " + requestDto.name, 0);
                return true;
            }
            return false;
        }
    }
}
