using AutoMapper;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO.CreditNote;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using ires.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class CreditNoteRepository : ICreditNoteService
    {
        private readonly DataContext _dataContext;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public CreditNoteRepository(DataContext dataContext, ILogService logService, IMapper mapper)
        {
            _dataContext = dataContext;
            _logService = logService;
            _mapper = mapper;
        }
        public async Task<CreditMemoTypeViewModel> CreateType(CreditMemoTypeRequestDto requestDto)
        {
            var entity = _mapper.Map<CreditMemoType>(requestDto);
            entity.id = 0;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.creditMemoTypes.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(entity.companyid, requestDto.createdbyid, AppModule.CreditNotes, "Create Credit Memo Type", "ID : " + entity.id + "-" + entity.name, 0);
            return _mapper.Map<CreditMemoTypeViewModel>(entity);
        }


        public async Task<CreditMemoType> GetTypeByID(long id)
        {
            return await _dataContext.creditMemoTypes.FindAsync(id);
        }
        public async Task<CreditMemoTypeViewModel> GetType(long id)
        {
            return _mapper.Map<CreditMemoTypeViewModel>(await GetTypeByID(id));
        }

        public async Task<CreditMemoTypeViewModel> GetTypeByName(int companyID, string name)
        {
            var result = await _dataContext.creditMemoTypes.Where(x => x.companyid == companyID && x.name == name).FirstOrDefaultAsync();
            return _mapper.Map<CreditMemoTypeViewModel>(result);
        }

        public async Task<ICollection<CreditMemoTypeViewModel>> GetTypes(int companyID, string search, bool viewAll)
        {
            var result = await _dataContext.creditMemoTypes.Where(x => x.companyid == companyID && x.name.Contains(search) && (x.isactive || viewAll)).OrderBy(x => x.name).ToListAsync();
            if (result.Count == 0 && search == "")
            {
                var seeder = new CreditMemoTypeSeeder(_dataContext);
                await seeder.Seed(companyID);
                return await GetTypes(companyID, search, viewAll);
            }
            return _mapper.Map<ICollection<CreditMemoTypeViewModel>>(result);
        }

        public async Task<bool> UpdateType(CreditMemoTypeRequestDto requestDto)
        {
            var entity = await GetTypeByID(requestDto.id);
            if (entity != null)
            {
                entity.name = requestDto.name;
                entity.isactive = requestDto.isactive;
                entity.updatedbyid = requestDto.updatedbyid;
                entity.dateupdated = Utility.GetServerTime();
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(entity.companyid, requestDto.updatedbyid, AppModule.CreditNotes, "Update Credit Note Type", "ID : " + entity.id + "-" + entity.name, 0);
                return true;
            }
            return false;
        }
    }
}
