using AutoMapper;
using ires_api.Data;
using ires_api.DTO.Client;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
{
    public class ClientRepository : IClientService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ClientRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<Client> Create(ClientDto requestDto)
        {
            Client client = _mapper.Map<Client>(requestDto);
            client.custid = 0;
            client.datecreated = DateTime.Now;
            _dataContext.clients.Add(client);
            await _dataContext.SaveChangesAsync();
            return client;
        }

        public async Task<Client> GetClientById(long id)
        {
            return await _dataContext.clients.FindAsync(id);
        }

        public async Task<Client> GetClientByName(int companyid, string lname, string fname)
        {
            return await _dataContext.clients.Where(x => x.companyid == companyid && x.lname == lname && x.fname == fname).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Client>> GetClients(int companyid, string search)
        {
            return await _dataContext.clients.Where(x => x.companyid == companyid && (x.lname.Contains(search) || x.fname.Contains(search)))
                .OrderBy(x => x.lname + x.fname).ToListAsync();
        }

        public async Task<int> GetCountClientAsync(int companyid)
        {
            return await _dataContext.clients.Where(x => x.companyid == companyid).CountAsync();
        }

        public async Task<Client> Update(ClientDto requestDto)
        {
            Client client = await GetClientById(requestDto.custid);
            if (client != null)
            {
                client.lname = requestDto.lname;
                client.fname = requestDto.fname;
                client.mname = requestDto.mname;
                client.birthdate = requestDto.birthdate;
                client.address = requestDto.address;
                client.contactno = requestDto.contactno;
                client.tinnumber = requestDto.tinnumber;
                client.email = requestDto.email;
                client.updatedbyid = requestDto.updatedbyid;
                client.dateupdated = DateTime.Now;
                await _dataContext.SaveChangesAsync();
            }
            return client;
        }
    }
}
