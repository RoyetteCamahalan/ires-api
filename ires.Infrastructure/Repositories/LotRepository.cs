using AutoMapper;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.Enumerations;
using ires.Domain.Models;
using ires.Infrastructure.Common;
using ires.Infrastructure.Data;
using ires.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ires.Infrastructure.Repositories
{
    public class LotRepository(DataContext _dataContext, IMapper _mapper) : ILotService
    {
        //public Task<PaginatedResult<Lot>> GetAvailableLotByProject(long projectID, PaginationRequest request)
        //{
        //    var result = await _dataContext.lots.Include(x => x.project).Where(x => x.project.companyid == _currentUserService.companyid
        //        && x.status == RentalPropertyStatus.Vacant && (x.propertyname.Contains(search) || x.area.Contains(search)))
        //        .OrderBy(x => x.project.propertyname).ThenBy(x => x.propertyname).ToListAsync();
        //    return _mapper.Map<ICollection<RentalUnit>>(result);
        //}

        public async Task<PaginatedResult<Lot>> GetLotsByProject(long projectID, PaginationRequest request)
        {
            var query = _dataContext.lots.Where(x => x.propertyid == projectID && x.name.Contains(request.Search))
                .OrderBy(x => x.name).AsQueryable();
            return await query.AsPaginatedResult<Entities.Lot, Lot>(request, _mapper);
        }
    }
}
