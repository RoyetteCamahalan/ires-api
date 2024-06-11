using ires_api.DTO.Client;

namespace ires_api.Services.Interface
{
    public interface IClientService
    {
        public Task<ClientViewModel> Create(ClientRequestDto requestDto);
        public Task<bool> Update(ClientRequestDto requestDto);
        public Task<ClientViewModel> GetByID(long id);
        public Task<ICollection<ClientViewModel>> GetClients(int companyid, string search);
        public Task<ClientViewModel> GetClientByName(int companyid, string lname, string fname);
        public Task<int> GetCountClientAsync(int companyid);
    }
}
