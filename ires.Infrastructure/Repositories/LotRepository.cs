using AutoMapper;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Domain.Models;
using ires.Infrastructure.Common;
using ires.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ires.Infrastructure.Repositories
{
    public class LotRepository(DataContext _dataContext, IMapper _mapper, ICurrentUserService _currentUserService, ILogService _logService) : ILotService
    {
        public async Task<Lot> Create(Lot request)
        {
            var entity = _mapper.Map<Entities.Lot>(request);
            entity.status = LotStatus.Vacant;
            _dataContext.lots.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(_currentUserService.companyid, _currentUserService.employeeid, AppModule.Projects, "Create Lot", "Create New Record : " + entity.lot_id.ToString() + "-" + entity.name, 0);
            return _mapper.Map<Lot>(entity);
        }

        public async Task<PaginatedResult<Lot>> GetAvailableLotByProject(Guid project_guid, PaginationRequest request)
        {
            var query = _dataContext.lots.Include(x => x.project).Where(x => x.project.guid == project_guid
                && x.status == LotStatus.Vacant && x.name.Contains(request.Search ?? ""))
                .OrderBy(x => x.name).AsQueryable();
            return await query.AsPaginatedResult<Entities.Lot, Lot>(request, _mapper);
        }
        private async Task<Entities.Lot> FindLotById(long id)
        {
            return await _dataContext.lots.FindAsync(id) ?? throw new ObjectNotFoundException();
        }
        public async Task<Lot> GetLotById(long id)
        {
            var entity = await FindLotById(id);
            return _mapper.Map<Lot>(entity);
        }

        public async Task<PaginatedResult<Lot>> GetLotsByProjectId(long projectId, PaginationRequest request)
        {
            var query = _dataContext.lots.Include(x => x.lotModel).Where(x => x.propertyid == projectId && x.name.Contains(request.Search ?? ""))
                .OrderBy(x => x.name).AsQueryable();
            return await query.AsPaginatedResult<Entities.Lot, Lot>(request, _mapper);
        }

        public async Task<bool> IsNameUnique(long projectId, string name)
        {
            return !await _dataContext.lots.Where(x => x.propertyid == projectId && x.name == name).AnyAsync();
        }

        public async Task Update(Lot request)
        {
            var entity = await FindLotById(request.lot_id);
            entity.blocknoint = request.blocknoint;
            entity.lotnoint = request.lotnoint;
            entity.name = request.name;
            entity.area = request.area;
            entity.pricepersquare = request.pricepersquare;
            entity.default_price = request.default_price;
            entity.compercentage = request.compercentage;
            entity.commissionableamount = request.commissionableamount;
            entity.comatdown = request.comatdown;
            entity.titleno = request.titleno;
            entity.model_id = request.model_id;
            if (entity.status != LotStatus.InContract && request.status != LotStatus.InContract)
                entity.status = request.status;
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(_currentUserService.companyid, _currentUserService.employeeid, AppModule.Projects, "Updated Lot", "Updated Record : " + entity.lot_id.ToString() + "-" + entity.name, 0);
        }
    }
}
