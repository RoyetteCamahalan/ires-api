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
        public async Task<ClientViewModel> Create(ClientRequestDto requestDto)
        {
            var entity = _mapper.Map<Client>(requestDto);
            entity.custid = 0;
            entity.datecreated = DateTime.Now;
            _dataContext.clients.Add(entity);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<ClientViewModel>(entity);
        }

        private async Task<Client> GetClientByID(long id)
        {
            return await _dataContext.clients.FindAsync(id);
        }

        public async Task<ClientViewModel> GetByID(long id)
        {
            var result = await GetClientByID(id);
            return _mapper.Map<ClientViewModel>(result);
        }

        public async Task<ClientViewModel> GetClientByName(int companyid, string lname, string fname)
        {
            var result = await _dataContext.clients.Where(x => x.companyid == companyid && x.lname == lname && x.fname == fname).FirstOrDefaultAsync();
            return _mapper.Map<ClientViewModel>(result);
        }

        public async Task<ICollection<ClientViewModel>> GetClients(int companyid, string search)
        {
            var result = await _dataContext.clients.Where(x => x.companyid == companyid && (x.lname.Contains(search) || x.fname.Contains(search)))
                .OrderBy(x => x.lname + x.fname).ToListAsync();
            return _mapper.Map<ICollection<ClientViewModel>>(result);
        }

        public async Task<int> GetCountClientAsync(int companyid)
        {
            return await _dataContext.clients.Where(x => x.companyid == companyid).CountAsync();
        }

        public async Task<bool> Update(ClientRequestDto requestDto)
        {
            var entity = await GetClientByID(requestDto.custid);
            if (entity != null)
            {
                entity.lname = requestDto.lname;
                entity.fname = requestDto.fname;
                entity.mname = requestDto.mname;
                entity.birthdate = requestDto.birthdate;
                entity.address = requestDto.address;
                entity.contactno = requestDto.contactno;
                entity.tinnumber = requestDto.tinnumber;
                entity.email = requestDto.email;
                entity.updatedbyid = requestDto.updatedbyid;
                entity.dateupdated = DateTime.Now;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
