using AutoMapper;
using ires.Infrastructure.Data;
using ires.Domain.Enumerations;
using ires.Infrastructure.Entities;
using ires.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using ires.Domain.DTO.RentalContract;
using ires.Domain.DTO.RentalContractDetail;
using ires.Domain.DTO.RentalUnit;

namespace ires.Infrastructure.Repositories
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
            entity.advancerent = entity.noofmonthadvance * entity.montlyrent;
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
            await RecomputeContract(entity.contractid);
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

        public async Task<ICollection<RentalContractDetailViewModel>> GetDetails(long contractID)
        {
            var result = await _dataContext.rentalContractDetails.Include(x => x.rentalProperty).ThenInclude(x => x.project).Where(x => x.contractid == contractID)
                .OrderBy(x => x.rentalProperty.project.propertyname).ToListAsync();
            return _mapper.Map<ICollection<RentalContractDetailViewModel>>(result);
        }

        public async Task<ICollection<RentalUnitViewModel>> GetProperties(long contractID)
        {
            var result = await _dataContext.rentalContractDetails.Include(x => x.rentalProperty).ThenInclude(x => x.project).Where(x => x.contractid == contractID)
                .Select(x => x.rentalProperty).OrderBy(x => x.project.propertyname).ToListAsync();
            return _mapper.Map<ICollection<RentalUnitViewModel>>(result);
        }

        public async Task<string> GetPropertiesAsString(long contractID)
        {
            long lastProjectID = 0;
            var strings = new List<string>();
            var details = await GetProperties(contractID);
            foreach (var property in details)
            {
                if (lastProjectID != property.projectid)
                {
                    if (lastProjectID > 0)
                        strings[^1] += ")";
                    lastProjectID = property.projectid;
                    strings.Add(property.project.propertyname + "(" + property.propertyname);
                }
                else
                    strings.Add(", " + property.propertyname);
            }
            return string.Join(" ", strings) + ")";
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
                entity.noofmonthadvance = requestDto.noofmonthadvance ?? 0;
                entity.deposit = requestDto.deposit ?? 0;
                entity.advancerent = entity.noofmonthadvance * entity.montlyrent;
                entity.remarks = requestDto.remarks;
                var details = await GetContractDetails(entity.contractid);
                foreach (var item in details)
                {
                    if (!requestDto.rentalContractDetails.Where(x => x.propertyid == item.propertyid).Any())
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
                        detail.id = 0;
                        detail.contractid = requestDto.contractid;
                        _dataContext.rentalContractDetails.Add(detail);
                        await _projectService.UpdateRentalUnitStatus(detail.propertyid, RentalPropertyStatus.Occupied);
                    }
                }
                await _dataContext.SaveChangesAsync();
                await RecomputeContract(requestDto.contractid);
                return true;
            }
            return false;
        }

        private async Task<RentalContract> GetContractById(long id)
        {
            return await _dataContext.rentalContracts.Include(x => x.client).Where(x => x.contractid == id).FirstOrDefaultAsync();
        }
        private async Task<ICollection<RentalContractDetail>> GetContractDetails(long contractID)
        {
            return await _dataContext.rentalContractDetails.Where(x => x.contractid == contractID).ToListAsync();
        }

        public async Task RecomputeContract(long contractID)
        {
            await Task.Run(() =>
                _dataContext.Database.ExecuteSqlRawAsync("exec spComputeRentals @contractid = {0}", contractID));
        }

        public async Task<ICollection<RentalHistoryViewModel>> GetAccountHistory(long companyID, long contractID)
        {
            return await _dataContext.Database.SqlQuery<RentalHistoryViewModel>($"exec spWebReports @contractid = {contractID}, @companyid =  {companyID}").ToListAsync();
        }
    }
}
