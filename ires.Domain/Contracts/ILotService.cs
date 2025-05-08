using ires.Domain.Common;
using ires.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Domain.Contracts
{
    public interface ILotService
    {
        public Task<PaginatedResult<Lot>> GetLotsByProject(long projectID, PaginationRequest request);
        //public Task<PaginatedResult<Lot>> GetAvailableLotByProject(long projectID, PaginationRequest request);
    }
}
