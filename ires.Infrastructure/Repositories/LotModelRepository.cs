using AutoMapper;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Domain.Models;
using ires.Infrastructure.Common;
using ires.Infrastructure.Data;
using ires.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Infrastructure.Repositories
{
    public class LotModelRepository(DataContext _dataContext, IMapper _mapper, ICurrentUserService _currentUserService, ILogService _logService) : ILotModelService
    {
        public async Task<LotModel> Create(LotModel request)
        {
            var entity = _mapper.Map<Entities.LotModel>(request);
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.lotModels.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(_currentUserService.companyid, _currentUserService.employeeid, AppModule.Projects, "Create Model", "Create New Record : " + entity.id + " - " + entity.name, 0);
            return _mapper.Map<LotModel>(entity);
        }

        private async Task<Entities.LotModel> FindLotModelById(long id)
        {
            return await _dataContext.lotModels.FindAsync(id) ?? throw new ObjectNotFoundException();
        }

        public async Task<LotModel> GetLotModelById(long id)
        {
            return _mapper.Map<LotModel>(await FindLotModelById(id));
        }

        public async Task<PaginatedResult<LotModel>> GetLotModelByProjectId(long id, PaginationRequest request)
        {
            var query = _dataContext.lotModels.Where(x => x.project_id == id && x.name.Contains(request.Search ?? ""))
                .OrderBy(x => x.name).AsQueryable();
            return await query.AsPaginatedResult<Entities.LotModel, LotModel>(request, _mapper);
        }

        public async Task<bool> IsNameUnique(long projectId, string name)
        {
            return !await _dataContext.lotModels.Where(x => x.project_id == projectId && x.name == name).AnyAsync();
        }

        public async Task Update(LotModel request)
        {
            var entity = await FindLotModelById(request.id);
            entity.name = request.name;
            entity.updatedbyid = _currentUserService.employeeid;
            entity.dateupdated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(_currentUserService.companyid, _currentUserService.employeeid, AppModule.Projects, "Update Model", "Record : " + entity.id + " - " + entity.name, 0);
        }
    }
}
