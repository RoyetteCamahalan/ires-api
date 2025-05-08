using ires.Domain.Common;
using ires.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Domain.Contracts
{
    public interface ILotModelService
    {
        public Task<LotModel> Create(LotModel request);
        public Task Update(LotModel request);
        public Task<LotModel> GetLotModelById(long id);
        public Task<PaginatedResult<LotModel>> GetLotModelByProjectId(long id, PaginationRequest request);
        public Task<bool> IsNameUnique(long projectId, string name);
    }
}
