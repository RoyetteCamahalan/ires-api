using AutoMapper;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.OtherFee;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Infrastructure.Extensions;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;

namespace ires.Infrastructure.Repositories
{
    public class OtherChargeRepository(
        DataContext _dataContext,
        IMapper _mapper,
        ILogService _logService,
        ICurrentUserService _currentUserService) : IOtherChargeService
    {

        public async Task<OtherFeeViewModel> CreateOtherFee(OtherFeeRequestDto requestDto)
        {
            var entity = _mapper.Map<OtherFee>(requestDto);
            entity.id = 0;
            entity.companyid = _currentUserService.companyid;
            entity.createdby = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.otherFees.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.OtherFees, "Create Other Fee", "Create New Record : " + entity.id.ToString() + " - " + entity.name);
            return _mapper.Map<OtherFeeViewModel>(entity);
        }
        private async Task<OtherFee> GetOtherFeeById(long id)
        {
            return await _dataContext.otherFees.FindAsync(id) ?? throw new EntityNotFoundException();
        }

        public async Task<OtherFeeViewModel> GetOtherFee(long id)
        {
            return _mapper.Map<OtherFeeViewModel>(await GetOtherFeeById(id));
        }

        public async Task<PaginatedResult<OtherFeeViewModel>> GetOtherFees(PaginationRequest request)
        {
            var query = _dataContext.otherFees.Where(x => x.companyid == _currentUserService.companyid &&
                (x.isactive || request.viewAll) && x.name.Contains(request.searchString))
                .OrderBy(x => x.name).AsQueryable();
            return await query.AsPaginatedResult<OtherFee, OtherFeeViewModel>(request, _mapper.ConfigurationProvider);
        }

        public async Task UpdateOtherFee(OtherFeeRequestDto requestDto)
        {
            var entity = await GetOtherFeeById(requestDto.id);
            entity.name = requestDto.name;
            entity.price = requestDto.price ?? 0;
            entity.description = requestDto.description;
            entity.isactive = requestDto.isactive;
            entity.updatedbyid = _currentUserService.employeeid;
            entity.dateupdated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.OtherFees, "Update Other Fee", "Update Record : " + requestDto.id.ToString() + " - " + requestDto.name);
        }
    }
}
