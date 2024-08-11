using ires.Domain.Common;
using ires.Domain.Models;

namespace ires.Domain.Contracts
{
    public interface IClientService
    {
        public Task<Client> Create(Client request);
        public Task Update(Client request);
        public Task<Client> GetByID(long id);
        public Task<PaginatedResult<Client>> GetClients(PaginationRequest request);
        public Task<bool> IsClientNameUnique(string lname, string fname);
        public Task<int> GetCountClientAsync();
    }
}
