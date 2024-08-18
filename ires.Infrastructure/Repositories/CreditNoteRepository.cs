using AutoMapper;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.CreditNote;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Infrastructure.Extensions;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using ires.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class CreditNoteRepository(DataContext _dataContext,
        ICurrentUserService _currentUserService,
        ILogService _logService,
        IMapper _mapper) : ICreditNoteService
    {

        public async Task<CreditMemoTypeViewModel> CreateType(CreditMemoTypeRequestDto requestDto)
        {
            var entity = _mapper.Map<CreditMemoType>(requestDto);
            entity.id = 0;
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.creditMemoTypes.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.CreditNotes, "Create Credit Memo Type", "ID : " + entity.id + "-" + entity.name);
            return _mapper.Map<CreditMemoTypeViewModel>(entity);
        }


        public async Task<CreditMemoType> GetTypeByID(long id)
        {
            return await _dataContext.creditMemoTypes.FindAsync(id) ?? throw new EntityNotFoundException();
        }
        public async Task<CreditMemoTypeViewModel> GetType(long id)
        {
            return _mapper.Map<CreditMemoTypeViewModel>(await GetTypeByID(id));
        }

        public async Task<CreditMemoTypeViewModel> GetTypeByName(string name)
        {
            var result = await _dataContext.creditMemoTypes.Where(x => x.companyid == _currentUserService.companyid && x.name == name).FirstOrDefaultAsync();
            return _mapper.Map<CreditMemoTypeViewModel>(result);
        }

        public async Task<PaginatedResult<CreditMemoTypeViewModel>> GetTypes(PaginationRequest request)
        {
            var query = _dataContext.creditMemoTypes.Where(x => x.companyid == _currentUserService.companyid &&
                x.name.Contains(request.searchString) && (x.isactive || request.viewAll)).OrderBy(x => x.name).AsQueryable();
            if (!query.Any() && request.search == "")
            {
                var seeder = new CreditMemoTypeSeeder(_dataContext);
                await seeder.Seed(_currentUserService.companyid);
                return await GetTypes(request);
            }
            return await query.AsPaginatedResult<CreditMemoType, CreditMemoTypeViewModel>(request, _mapper.ConfigurationProvider);
        }

        public async Task UpdateType(CreditMemoTypeRequestDto requestDto)
        {
            var entity = await GetTypeByID(requestDto.id); entity.name = requestDto.name;
            entity.isactive = requestDto.isactive;
            entity.updatedbyid = _currentUserService.employeeid;
            entity.dateupdated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.CreditNotes, "Update Credit Note Type", "ID : " + entity.id + "-" + entity.name);
        }
    }
}
