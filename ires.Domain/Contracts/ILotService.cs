using ires.Domain.Common;
using ires.Domain.Models;

namespace ires.Domain.Contracts
{
    public interface ILotService
    {
        public Task<Lot> Create(Lot request);
        public Task Update(Lot request);
        public Task<Lot> GetLotById(long id);
        public Task<PaginatedResult<Lot>> GetLotsByProjectId(long projectId, PaginationRequest request);
        public Task<PaginatedResult<Lot>> GetAvailableLotByProject(Guid project_guid, PaginationRequest request);
        public Task<bool> IsNameUnique(long projectId, string name);
    }
}
