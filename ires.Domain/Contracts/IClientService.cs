using ires.Domain.Common;
using ires.Domain.DTO.Client;

namespace ires.Domain.Contracts
{
    public interface IClientService
    {
        public Task<ClientViewModel> Create(ClientRequestDto requestDto);
        public Task Update(ClientRequestDto requestDto);
        public Task<ClientViewModel> GetByID(long id);
        public Task<PaginatedResult<ClientViewModel>> GetClients(PaginationRequest request);
        public Task<ClientViewModel> GetClientByName(string lname, string fname);
        public Task<int> GetCountClientAsync();
    }
}
