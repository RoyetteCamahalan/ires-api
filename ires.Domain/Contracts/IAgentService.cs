using ires.Domain.Common;
using ires.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Domain.Contracts
{
    public interface IAgentService
    {
        public Task<PaginatedResult<Agent>> GeAgents(PaginationRequest request);
        public Task<Agent> FindAgentByGuid(Guid guid);
        public Task<Agent> FindAgentByID(long id);
        public Task<Agent> Create(Agent agent);
        public Task Update(Agent agent);
        public Task<bool> IsNameUnique(Guid guid, string firstname, string lastname);
    }
}
