using AutoMapper;
using AutoMapper.QueryableExtensions;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.Exceptions;
using ires.Domain.Models;
using ires.Infrastructure.Common;
using ires.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace ires.Infrastructure.Repositories
{
    public class AgentRepository(DataContext _dataContext, ICurrentUserService _currentUserService, IMapper _mapper) : IAgentService
    {
        public async Task<Agent> Create(Agent agent)
        {
            var entity = _mapper.Map<Entities.Agent>(agent);
            entity.guid = Guid.NewGuid();
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.agents.Add(entity);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<Agent>(entity);
        }

        public async Task<Agent> FindAgentByGuid(Guid guid)
        {
            var entity = await _dataContext.agents.Include(x => x.upline).Where(x => x.guid == guid).FirstOrDefaultAsync() ?? throw new ObjectNotFoundException();
            return _mapper.Map<Agent>(entity);
        }

        public async Task<Agent> FindAgentByID(long id)
        {
            var entity = await _dataContext.agents.Include(x => x.upline).Where(x => x.id == id).FirstOrDefaultAsync() ?? throw new ObjectNotFoundException();
            return _mapper.Map<Agent>(entity);
        }

        public async Task<PaginatedResult<Agent>> GeAgents(PaginationRequest request)
        {
            var query = _dataContext.agents.Include(x => x.upline)
                .Where(x => x.companyid == _currentUserService.companyid 
                    && ((x.firstname + ' ' + x.lastname).Contains(request.Search ?? "") || x.email.Contains(request.Search ?? "")))
                .OrderBy(x => x.lastname).ThenBy(x => x.firstname).AsQueryable();
            return await query.AsPaginatedResult<Entities.Agent, Agent>(request, _mapper);
        }

        public async Task<bool> IsNameUnique(Guid guid, string firstname, string lastname)
        {
            return !await _dataContext.agents.Where(x => x.companyid == _currentUserService.companyid
                && x.firstname == firstname && x.lastname == lastname && x.guid != guid).AnyAsync();
        }

        public async Task Update(Agent agent)
        {
            var entity = await _dataContext.agents.Where(x => x.guid == agent.guid).FirstOrDefaultAsync() ?? throw new ObjectNotFoundException();
            entity.firstname = agent.firstname;
            entity.lastname = agent.lastname;
            entity.contactno = agent.contactno;
            entity.address = agent.address;
            entity.email = agent.email;
            entity.tinnumber = agent.tinnumber;
            entity.isactive = agent.isactive;
            entity.upline_id = agent.upline_id == 0 ? null : agent.upline_id;
            entity.updatedbyid = _currentUserService.employeeid;
            entity.dateupdated = Utility.GetServerTime();
            await _dataContext.SaveChangesAsync();
        }
    }
}
