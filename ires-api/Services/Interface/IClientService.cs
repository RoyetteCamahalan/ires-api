using ires_api.DTO.Client;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IClientService
    {
        public Task<Client> Create(ClientDto requestDto);
        public Task<Client> Update(ClientDto requestDto);
        public Task<Client> GetClientById(long id);
        public Task<ICollection<Client>> GetClients(int companyid, string search);
        public Task<Client> GetClientByName(int companyid, string lname, string fname);
        public Task<int> GetCountClientAsync(int companyid);
    }
}
