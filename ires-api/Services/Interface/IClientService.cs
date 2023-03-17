using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IClientService
    {
        public Client Create(ClientDto requestDto);
        public Client Update(ClientDto requestDto);
        public Client GetClientById(long id);
        public ICollection<Client> GetClients(int companyid, string search);
        public Client GetClientByName(int companyid, string lname, string fname);
    }
}
