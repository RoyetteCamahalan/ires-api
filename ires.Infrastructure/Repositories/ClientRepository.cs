using AutoMapper;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Domain.Models;
using ires.Infrastructure.Common;
using ires.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class ClientRepository(DataContext _dataContext, IMapper _mapper,
        ICurrentUserService _currentUserService,
        ILogService _logService) : IClientService
    {

        public async Task<Client> Create(Client request)
        {
            var entity = _mapper.Map<Entities.Client>(request);
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.clients.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Clients, "Create Record", $"New Client: {entity.custid} - {entity.fname} {entity.lname}");
            return _mapper.Map<Client>(entity);
        }

        private async Task<Entities.Client> GetClientByID(long id)
        {
            return await _dataContext.clients.FindAsync(id) ?? throw new EntityNotFoundException();
        }

        public async Task<Client> GetByID(long id)
        {
            var result = await GetClientByID(id);
            return _mapper.Map<Client>(result);
        }

        public async Task<bool> IsClientNameUnique(string lname, string fname)
        {
            return !await _dataContext.clients.Where(x => x.companyid == _currentUserService.companyid
                && x.lname == lname && x.fname == fname).AnyAsync();
        }

        public async Task<PaginatedResult<Client>> GetClients(PaginationRequest request)
        {
            var query = _dataContext.clients.Where(x => x.companyid == _currentUserService.companyid
                && (x.lname.Contains(request.Search) || x.fname.Contains(request.Search)))
                .OrderBy(x => x.fname + x.lname).AsQueryable();
            return await query.AsPaginatedResult<Entities.Client, Client>(request, _mapper.ConfigurationProvider);
        }

        public async Task<int> GetCountClientAsync()
        {
            return await _dataContext.clients.Where(x => x.companyid == _currentUserService.companyid).CountAsync();
        }

        public async Task Update(Client request)
        {
            var entity = await GetClientByID(request.custid);
            entity.lname = request.lname;
            entity.fname = request.fname;
            entity.mname = request.mname;
            entity.birthdate = request.birthdate;
            entity.address = request.address;
            entity.contactno = request.contactno;
            entity.tinnumber = request.tinnumber;
            entity.email = request.email;
            entity.updatedbyid = _currentUserService.employeeid;
            entity.dateupdated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Clients, "Update Record", $"Client: {entity.custid} - {entity.fname} {entity.lname}");
        }
    }
}
