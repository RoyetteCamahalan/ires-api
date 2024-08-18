using AutoMapper;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.Client;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Infrastructure.Extensions;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class ClientRepository(DataContext _dataContext, IMapper _mapper,
        ICurrentUserService _currentUserService,
        ILogService _logService) : IClientService
    {
        public async Task<ClientViewModel> Create(ClientRequestDto requestDto)
        {
            var entity = _mapper.Map<Client>(requestDto);
            entity.custid = 0;
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.clients.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Clients, "New Client", $"New Record: {entity.custid} - {entity.fname} {entity.lname}");
            return _mapper.Map<ClientViewModel>(entity);
        }

        private async Task<Client> GetClientByID(long id)
        {
            return await _dataContext.clients.FindAsync(id) ?? throw new EntityNotFoundException();
        }

        public async Task<ClientViewModel> GetByID(long id)
        {
            var result = await GetClientByID(id);
            return _mapper.Map<ClientViewModel>(result);
        }

        public async Task<ClientViewModel> GetClientByName(string lname, string fname)
        {
            var result = await _dataContext.clients.Where(x => x.companyid == _currentUserService.companyid && x.lname == lname && x.fname == fname).FirstOrDefaultAsync();
            return _mapper.Map<ClientViewModel>(result);
        }

        public async Task<PaginatedResult<ClientViewModel>> GetClients(PaginationRequest request)
        {
            var query = _dataContext.clients.Where(x => x.companyid == _currentUserService.companyid && (x.lname.Contains(request.searchString) || x.fname.Contains(request.searchString)))
                .OrderBy(x => x.fname + x.lname).AsQueryable();
            return await query.AsPaginatedResult<Client, ClientViewModel>(request, _mapper.ConfigurationProvider);
        }

        public async Task<int> GetCountClientAsync()
        {
            return await _dataContext.clients.Where(x => x.companyid == _currentUserService.companyid).CountAsync();
        }

        public async Task Update(ClientRequestDto requestDto)
        {
            var entity = await GetClientByID(requestDto.custid);
            entity.lname = requestDto.lname;
            entity.fname = requestDto.fname;
            entity.mname = requestDto.mname;
            entity.birthdate = requestDto.birthdate;
            entity.address = requestDto.address;
            entity.contactno = requestDto.contactno;
            entity.tinnumber = requestDto.tinnumber;
            entity.email = requestDto.email;
            entity.updatedbyid = _currentUserService.employeeid;
            entity.dateupdated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(AppModule.Clients, "Update Client", $"Record: {entity.custid} - {entity.fname} {entity.lname}");
        }
    }
}
