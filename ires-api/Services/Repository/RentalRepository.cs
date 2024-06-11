using AutoMapper;
using ires_api.Data;
using ires_api.DTO.RentalContract;
using ires_api.DTO.RentalUnit;
using ires_api.Enumerations;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
{
    public class RentalRepository : IRentalService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;

        public RentalRepository(DataContext dataContext, IMapper mapper, IProjectService projectService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _projectService = projectService;
        }
        public async Task<RentalContractViewModel> Create(RentalContractRequestDto requestDto)
        {
            var entity = _mapper.Map<RentalContract>(requestDto);
            entity.contractid = 0;
            entity.status = RentStatus.Active;
            entity.datecreated = DateTime.Now;
            entity.contractno = (_dataContext.rentalContracts.Max(x => (long?)x.contractno) ?? 0) + 1;
            entity.rentalContractDetails = new List<RentalContractDetail>();
            foreach (var item in requestDto.rentalContractDetails)
            {
                entity.rentalContractDetails.Add(new RentalContractDetail
                {
                    id = 0,
                    propertyid = item.propertyid,
                    datecreated = DateTime.Now,
                    createdbyid = entity.createdbyid
                });
            }
            _dataContext.rentalContracts.Add(entity);
            await _dataContext.SaveChangesAsync();
            foreach (var item in requestDto.rentalContractDetails)
            {
                await _projectService.UpdateRentalUnitStatus(item.propertyid, RentalPropertyStatus.Occupied);
            }
            return _mapper.Map<RentalContractViewModel>(entity);
        }

        public async Task<RentalContractViewModel> Get(long companyID, long contractID)
        {
            var entity = await GetContractById(contractID);
            if (entity != null)
                return _mapper.Map<RentalContractViewModel>(entity);
            return null;
        }

        public async Task<ICollection<RentalContractViewModel>> GetAll(long companyID, string search, int filterByID)
        {
            var result = await _dataContext.rentalContracts.Include(x => x.client).Include(x => x.rentalContractDetails).ThenInclude(x => x.rentalProperty)
                .Where(x => x.companyid == companyID
                && (filterByID == 2 || (int)x.status == filterByID)
                && (x.client.lname.Contains(search) || x.client.fname.Contains(search) || x.remarks.Contains(search)))
                .OrderByDescending(x => x.contractno).ToListAsync();
            return _mapper.Map<ICollection<RentalContractViewModel>>(result);
        }

        public async Task<RentalContractViewModel> GetContractByUnit(long propertyID)
        {
            var result = await _dataContext.rentalContractDetails.Include(x => x.rentalContract).ThenInclude(x => x.client)
                .Where(x => x.rentalContract.status == RentStatus.Active && x.propertyid == propertyID)
                .Select(x => x.rentalContract).FirstOrDefaultAsync();
            return _mapper.Map<RentalContractViewModel>(result);
        }

        public async Task<ICollection<RentalUnitViewModel>> GetProperties(long contractID)
        {
            var result = await _dataContext.rentalContractDetails.Include(x => x.rentalProperty).ThenInclude(x => x.project).Where(x => x.contractid == contractID)
                .Select(x => x.rentalProperty).OrderBy(x => x.project.propertyname).ToListAsync();
            return _mapper.Map<ICollection<RentalUnitViewModel>>(result);
        }

        public async Task<bool> Update(RentalContractRequestDto requestDto)
        {
            var entity = await GetContractById(requestDto.contractid);
            if (entity != null)
            {
                entity.custid = requestDto.custid;
                entity.contractdate = requestDto.contractdate;
                entity.billingstart = requestDto.billingstart;
                entity.term = requestDto.term ?? 0;
                entity.montlyrent = requestDto.montlyrent;
                entity.monthlypenalty = requestDto.monthlypenalty ?? 0;
                entity.penaltyextension = requestDto.penaltyextension ?? 0;
                entity.remarks = requestDto.remarks;
                var details = await GetContractDetails(entity.contractid);
                foreach (var item in details)
                {
                    if (requestDto.rentalContractDetails.Where(x => x.id == item.id).Any())
                    {
                        _dataContext.rentalContractDetails.Remove(item);
                        await _projectService.UpdateRentalUnitStatus(item.propertyid, RentalPropertyStatus.Vacant);
                    }
                }
                foreach (var item in requestDto.rentalContractDetails)
                {
                    if (!details.Where(x => x.propertyid == item.propertyid).Any())
                    {
                        var detail = _mapper.Map<RentalContractDetail>(item);
                        _dataContext.rentalContractDetails.Add(detail);
                        await _projectService.UpdateRentalUnitStatus(detail.propertyid, RentalPropertyStatus.Occupied);
                    }
                }
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private async Task<RentalContract> GetContractById(long id)
        {
            return await _dataContext.rentalContracts.FindAsync(id);
        }
        private async Task<ICollection<RentalContractDetail>> GetContractDetails(long contractID)
        {
            return await _dataContext.rentalContractDetails.Where(x => x.contractid == contractID).ToListAsync();
        }
    }
}
