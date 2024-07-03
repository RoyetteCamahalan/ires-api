using AutoMapper;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO.Client;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
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
            entity.datecreated = Utility.GetServerTime();
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
                .OrderBy(x => x.fname + x.lname).ToListAsync();
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
                entity.dateupdated = Utility.GetServerTime();
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
