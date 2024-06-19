using AutoMapper;
using ires.Domain.Contracts;
using ires.Domain.DTO.Payment;
using ires.Domain.DTO.RentalCharge;
using ires.Domain.DTO.RentalContract;
using ires.Domain.DTO.RentalContractDetail;
using ires.Domain.DTO.RentalUnit;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class RentalRepository : IRentalService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly ILogService _logService;

        public RentalRepository(DataContext dataContext, IMapper mapper, IProjectService projectService, ILogService logService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _projectService = projectService;
            _logService = logService;
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
            await _logService.SaveLogAsync(requestDto.companyid, requestDto.createdbyid, AppModule.Rentals, "Create Rental Contract", "Create New Record : " + entity.contractid, 0);
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
                && (filterByID == 2 || (int)x.status == filterByID || (filterByID == 3 && x.totalbalance > 0))
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
                await _logService.SaveLogAsync(entity.companyid, requestDto.updatedbyid, 0, "Update Rental Contract", "Update Record : " + requestDto.contractid.ToString(), 0);
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
            try
            {
                await _dataContext.Database.ExecuteSqlRawAsync("exec spComputeRentals @contractid = {0}", contractID);
            }
            catch (Exception) { }
        }

        public async Task<ICollection<RentalHistoryViewModel>> GetAccountHistory(long companyID, long contractID)
        {
            var result = await _dataContext.rentalAccountHistories.FromSqlRaw($"exec spWebReports @operation=0, @soperation=0, @contractid = {contractID}, @companyid =  {companyID}").ToListAsync();
            return _mapper.Map<ICollection<RentalHistoryViewModel>>(result);
        }
        public async Task<ICollection<PayableViewModel>> GetSOA(long companyID, long contractID)
        {
            var result = await _dataContext.payables.FromSqlRaw($"exec spWebReports @operation=0, @soperation=3, @contractid = {contractID}, @companyid =  {companyID}").ToListAsync();
            return _mapper.Map<ICollection<PayableViewModel>>(result);
        }





        private async Task<RentalCharge> GetRentalChargeByID(long id)
        {
            return await _dataContext.rentalCharges.Include(x => x.rentalContract).Include(x => x.otherFee).FirstOrDefaultAsync(x => x.chargeid == id);
        }

        public async Task<RentalChargeViewModel> CreateOtherCharge(RentalChargeRequestDto requestDto)
        {
            var entity = _mapper.Map<RentalCharge>(requestDto);
            entity.chargeid = 0;
            entity.chargetype = ChargeType.OtherFees;
            entity.datecreated = DateTime.Now;
            _dataContext.rentalCharges.Add(entity);
            await _dataContext.SaveChangesAsync();
            var contract = await GetContractById(requestDto.contractid);
            await _logService.SaveLogAsync(contract.companyid, entity.createdbyid, AppModule.Rentals, "Posted New Fee", "Created new record: " + entity.chargeid + " Amount: " + entity.chargeamount, 0);
            await RecomputeContract(requestDto.contractid);
            return _mapper.Map<RentalChargeViewModel>(entity);
        }

        public async Task<bool> UpdateOtherCharge(RentalChargeRequestDto requestDto)
        {
            var entity = await GetRentalChargeByID(requestDto.chargeid);
            if (entity != null)
            {
                entity.otherfeeid = requestDto.otherfeeid;
                entity.chargeamount = requestDto.chargeamount;
                entity.chargedate = requestDto.chargedate;
                entity.interestpercentage = requestDto.interestpercentage;
                entity.updatedbyid = requestDto.updatedbyid;
                entity.dateupdated = DateTime.Now;
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(entity.rentalContract.companyid, entity.createdbyid, AppModule.Rentals, "Update Other Fee", "Updated record: " + entity.chargeid + " Amount: " + entity.chargeamount, 0);
                await RecomputeContract(requestDto.contractid);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteOtherCharge(long id)
        {
            var entity = await GetRentalChargeByID(id);
            if (entity != null)
            {
                _dataContext.rentalCharges.Remove(entity);
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(entity.rentalContract.companyid, entity.createdbyid, AppModule.Rentals, "Removed Fee", "Removed record: " + entity.chargeid + " Name: " + entity.otherFee.name + " Amount: " + entity.chargeamount, 0);
                await RecomputeContract(entity.contractid);
                return true;
            }
            return false;
        }

        public async Task<RentalChargeViewModel> GetRentalCharge(long id)
        {
            return _mapper.Map<RentalChargeViewModel>(await GetRentalChargeByID(id));

        }

        public Task<bool> RentalChageHasPayment(long id)
        {
            return _dataContext.paymentDetails.Include(x => x.payment).Where(x => x.rentalchargeid == id && x.payment.status == PaymentStatus.paid).AnyAsync();
        }

        public async Task<bool> UpdateContractStatus(RentalTerminateRequestDto requestDto)
        {
            var entity = await GetContractById(requestDto.contractid);
            if (entity != null)
            {
                entity.status = requestDto.status;
                entity.dateterminated = requestDto.status == RentStatus.Inactive ? requestDto.dateterminated : null;
                entity.updatedbyid = requestDto.updatedbyid;
                var contractDetails = await _dataContext.rentalContractDetails.Include(x => x.rentalProperty).Where(x => x.contractid == requestDto.contractid).ToListAsync();
                foreach (var detail in contractDetails)
                {
                    if (requestDto.status == RentStatus.Inactive)
                    {
                        var hasOtherContract = await _dataContext.rentalContractDetails.Include(x => x.rentalContract).Where(x => x.rentalContract.status == RentStatus.Active && x.contractid != requestDto.contractid && x.propertyid == detail.propertyid).AnyAsync();
                        if (!hasOtherContract)
                            detail.rentalProperty.status = RentalPropertyStatus.Vacant;
                    }
                    else
                        detail.rentalProperty.status = RentalPropertyStatus.Occupied;
                }
                await _dataContext.SaveChangesAsync();
                if (requestDto.status == RentStatus.Inactive)
                    await _logService.SaveLogAsync(entity.companyid, requestDto.updatedbyid, AppModule.Rentals, "Contract Terminated", "Contract Terminated: " + entity.contractid, 0);
                else
                    await _logService.SaveLogAsync(entity.companyid, requestDto.updatedbyid, AppModule.Rentals, "Contract Reactivated", "Contract Reactivated: " + entity.contractid, 0);
                return true;
            }
            return false;
        }
    }
}
