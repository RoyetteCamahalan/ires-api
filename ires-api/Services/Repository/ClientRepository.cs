using AutoMapper;
using ires_api.Data;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;

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
        public Client Create(ClientDto requestDto)
        {
            Client client = _mapper.Map<Client>(requestDto);
            client.custid = 0;
            client.datecreated = DateTime.Now;
            _dataContext.clients.Add(client);
            _dataContext.SaveChanges();
            return client;
        }

        public Client GetClientById(long id)
        {
            return _dataContext.clients.Find(id);
        }

        public Client GetClientByName(int companyid, string lname, string fname)
        {
            return _dataContext.clients.Where(x => x.companyid == companyid && x.lname == lname && x.fname == fname).FirstOrDefault();
        }

        public ICollection<Client> GetClients(int companyid, string search)
        {
            return _dataContext.clients.Where(x => x.companyid == companyid && (x.lname.Contains(search) || x.fname.Contains(search)))
                .OrderBy(x => x.lname + x.fname).ToList();
        }

        public Client Update(ClientDto requestDto)
        {
            Client client = GetClientById(requestDto.custid);
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
                _dataContext.SaveChanges();
            }
            return client;
        }
    }
}
