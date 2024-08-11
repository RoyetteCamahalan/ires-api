using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using ires.Domain;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.Enumerations;
using ires.Domain.Exceptions;
using ires.Domain.Models;
using ires.Infrastructure.Common;
using ires.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ires.Infrastructure.Repositories
{
    public class RentalRepository(
        DataContext _dataContext,
        ICurrentUserService _currentUserService,
        IMapper _mapper,
        IProjectService _projectService,
        IConfiguration _configuration,
        IMailService _mailService,
        IConverter _converter,
        ILogService _logService) : IRentalService
    {

        public async Task<RentalContract> Create(RentalContract request)
        {
            var entity = _mapper.Map<Entities.RentalContract>(request);
            entity.status = RentStatus.Active;
            entity.datecreated = Utility.GetServerTime();
            entity.companyid = _currentUserService.companyid;
            entity.createdbyid = _currentUserService.employeeid;
            entity.contractno = (_dataContext.rentalContracts.Max(x => (long?)x.contractno) ?? 0) + 1;
            entity.advancerent = entity.noofmonthadvance * entity.montlyrent;
            entity.rentalContractDetails = [];
            foreach (var item in request.rentalContractDetails)
            {
                entity.rentalContractDetails.Add(new Entities.RentalContractDetail
                {
                    propertyid = item.propertyid,
                    datecreated = Utility.GetServerTime(),
                    createdbyid = entity.createdbyid
                });
            }
            _dataContext.rentalContracts.Add(entity);
            await _dataContext.SaveChangesAsync();
            await _logService.SaveLogAsync(_currentUserService.companyid, _currentUserService.employeeid, AppModule.Rentals, "Create Rental Contract", "Create New Record : " + entity.contractid, 0);
            foreach (var item in request.rentalContractDetails)
            {
                await _projectService.UpdateRentalUnitStatus(item.propertyid, RentalPropertyStatus.Occupied);
            }
            await RecomputeContract(entity.contractid);
            return _mapper.Map<RentalContract>(entity);
        }

        public async Task<RentalContract> Get(long contractID)
        {
            var entity = await GetContractById(contractID);
            return _mapper.Map<RentalContract>(entity);
        }

        public async Task<PaginatedResult<RentalContract>> GetAll(PaginationRequest request)
        {
            var query = _dataContext.rentalContracts.Include(x => x.client).Include(x => x.rentalContractDetails).ThenInclude(x => x.rentalProperty)
                .Where(x => x.companyid == _currentUserService.companyid
                && (request.filterByID == 2 || (int)x.status == request.filterByID || (request.filterByID == 3 && x.totalbalance > 0))
                && (x.client.lname.Contains(request.Search) || x.client.fname.Contains(request.Search) || x.remarks.Contains(request.Search)))
                .OrderByDescending(x => x.contractno).AsQueryable();
            return await query.AsPaginatedResult<Entities.RentalContract, RentalContract>(request, _mapper.ConfigurationProvider); ;
        }

        public async Task<ICollection<RentalContractDetail>> GetDetails(long contractID)
        {
            var result = await _dataContext.rentalContractDetails.Include(x => x.rentalProperty).ThenInclude(x => x.project).Where(x => x.contractid == contractID)
                .OrderBy(x => x.rentalProperty.project.propertyname).ToListAsync();
            return _mapper.Map<ICollection<RentalContractDetail>>(result);
        }

        public async Task<ICollection<RentalProperty>> GetProperties(long contractID)
        {
            var result = await _dataContext.rentalContractDetails.Include(x => x.rentalProperty).ThenInclude(x => x.project).Where(x => x.contractid == contractID)
                .Select(x => x.rentalProperty).OrderBy(x => x.project.propertyname).ToListAsync();
            return _mapper.Map<ICollection<RentalProperty>>(result);
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

        public async Task<bool> Update(RentalContract request)
        {
            var entity = await GetContractById(request.contractid);
            if (entity != null)
            {
                entity.custid = request.custid;
                entity.contractdate = request.contractdate;
                entity.billingsched = request.billingsched > 0 ? request.billingsched : entity.contractdate.Day;
                entity.term = request.term;
                entity.montlyrent = request.montlyrent;
                entity.monthlypenalty = request.monthlypenalty;
                entity.penaltyextension = request.penaltyextension;
                entity.noofmonthadvance = request.noofmonthadvance;
                entity.deposit = request.deposit;
                entity.advancerent = entity.noofmonthadvance * entity.montlyrent;
                entity.remarks = request.remarks;
                entity.updatedbyid = _currentUserService.employeeid;
                entity.dateupdated = Utility.GetServerTime();
                var details = await GetContractDetails(entity.contractid);
                foreach (var item in details)
                {
                    if (!request.rentalContractDetails.Where(x => x.propertyid == item.propertyid).Any())
                    {
                        _dataContext.rentalContractDetails.Remove(item);
                        await _projectService.UpdateRentalUnitStatus(item.propertyid, RentalPropertyStatus.Vacant);
                    }
                }
                foreach (var item in request.rentalContractDetails)
                {
                    if (!details.Where(x => x.propertyid == item.propertyid).Any())
                    {
                        var detail = _mapper.Map<Entities.RentalContractDetail>(item);
                        detail.id = 0;
                        detail.contractid = request.contractid;
                        _dataContext.rentalContractDetails.Add(detail);
                        await _projectService.UpdateRentalUnitStatus(detail.propertyid, RentalPropertyStatus.Occupied);
                    }
                }
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(AppModule.Rentals, "Update Rental Contract", "Update Record : " + request.contractid.ToString(), 0);
                await RecomputeContract(request.contractid);
                return true;
            }
            return false;
        }

        private async Task<Entities.RentalContract> GetContractById(long id)
        {
            return await _dataContext.rentalContracts.Include(x => x.client).Where(x => x.contractid == id).FirstOrDefaultAsync() ??
                throw new EntityNotFoundException();
        }
        private async Task<ICollection<Entities.RentalContractDetail>> GetContractDetails(long contractID)
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

        public async Task<ICollection<RentalAccountHistory>> GetAccountHistory(long contractID)
        {

            var result = await _dataContext.rentalAccountHistories
                .FromSqlRaw("exec spWebReports @operation = 0, @soperation = 0, @contractid = @contractId, @companyid = @companyId",
                new SqlParameter("@contractId", contractID),
                new SqlParameter("@companyId", _currentUserService.companyid))
            .ToListAsync();
            return _mapper.Map<ICollection<RentalAccountHistory>>(result);
        }
        public async Task<ICollection<Payable>> GetSOA(long contractID)
        {
            var result = await _dataContext.payables
                .FromSqlRaw("exec spWebReports @operation = 0, @soperation = 3, @contractid = @contractId, @companyid = @companyId",
                    new SqlParameter("@contractId", contractID),
                    new SqlParameter("@companyId", _currentUserService.companyid))
                .ToListAsync();
            return _mapper.Map<ICollection<Payable>>(result);
        }





        private async Task<Entities.RentalCharge> GetRentalChargeByID(long id)
        {
            return await _dataContext.rentalCharges.Include(x => x.rentalContract).Include(x => x.otherFee).FirstOrDefaultAsync(x => x.chargeid == id) ??
                throw new EntityNotFoundException();
        }

        public async Task<RentalCharge> CreateOtherCharge(RentalCharge request)
        {
            var entity = _mapper.Map<Entities.RentalCharge>(request);
            entity.chargetype = ChargeType.OtherFees;
            entity.createdbyid = _currentUserService.employeeid;
            entity.datecreated = Utility.GetServerTime();
            _dataContext.rentalCharges.Add(entity);
            await _dataContext.SaveChangesAsync();
            var contract = await GetContractById(request.contractid);
            await _logService.SaveLogAsync(AppModule.Rentals, "Posted New Fee", "Created new record: " + entity.chargeid + " Amount: " + entity.chargeamount, 0);
            await RecomputeContract(request.contractid);
            return _mapper.Map<RentalCharge>(entity);
        }

        public async Task<bool> UpdateOtherCharge(RentalCharge request)
        {
            var entity = await GetRentalChargeByID(request.chargeid);
            if (entity != null)
            {
                entity.otherfeeid = request.otherfeeid;
                entity.chargeamount = request.chargeamount;
                entity.chargedate = request.chargedate;
                entity.interestpercentage = request.interestpercentage;
                entity.updatedbyid = _currentUserService.employeeid;
                entity.dateupdated = Utility.GetServerTime();
                await _dataContext.SaveChangesAsync();
                await _logService.SaveLogAsync(AppModule.Rentals, "Update Other Fee", "Updated record: " + entity.chargeid + " Amount: " + entity.chargeamount, 0);
                await RecomputeContract(request.contractid);
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
                await _logService.SaveLogAsync(AppModule.Rentals, "Removed Fee", "Removed record: " + entity.chargeid + " Name: " + entity.otherFee.name + " Amount: " + entity.chargeamount, 0);
                await RecomputeContract(entity.contractid);
                return true;
            }
            return false;
        }

        public async Task<RentalCharge> GetRentalCharge(long id)
        {
            return _mapper.Map<RentalCharge>(await GetRentalChargeByID(id));

        }

        public Task<bool> RentalChageHasPayment(long id)
        {
            return _dataContext.paymentDetails.Include(x => x.payment).Where(x => x.rentalchargeid == id && x.payment.status == PaymentStatus.paid).AnyAsync();
        }

        public async Task<bool> UpdateContractStatus(RentalContract request)
        {
            var entity = await GetContractById(request.contractid);
            if (entity != null)
            {
                entity.status = request.status;
                entity.dateterminated = request.status == RentStatus.Inactive ? request.dateterminated : null;
                entity.updatedbyid = _currentUserService.employeeid;
                entity.dateupdated = Utility.GetServerTime();
                var contractDetails = await _dataContext.rentalContractDetails.Include(x => x.rentalProperty).Where(x => x.contractid == request.contractid).ToListAsync();
                foreach (var detail in contractDetails)
                {
                    if (request.status == RentStatus.Inactive)
                    {
                        var hasOtherContract = await _dataContext.rentalContractDetails.Include(x => x.rentalContract).Where(x => x.rentalContract.status == RentStatus.Active && x.contractid != request.contractid && x.propertyid == detail.propertyid).AnyAsync();
                        if (!hasOtherContract)
                            detail.rentalProperty.status = RentalPropertyStatus.Vacant;
                    }
                    else
                        detail.rentalProperty.status = RentalPropertyStatus.Occupied;
                }
                await _dataContext.SaveChangesAsync();
                if (request.status == RentStatus.Inactive)
                    await _logService.SaveLogAsync(entity.companyid, request.updatedbyid, AppModule.Rentals, "Contract Terminated", "Contract Terminated: " + entity.contractid, 0);
                else
                    await _logService.SaveLogAsync(entity.companyid, request.updatedbyid, AppModule.Rentals, "Contract Reactivated", "Contract Reactivated: " + entity.contractid, 0);
                return true;
            }
            return false;
        }

        public async Task<int> CountActiveUnits()
        {
            return await _dataContext.rentalProperties.Include(x => x.project)
                .Where(x => x.project.companyid == _currentUserService.companyid && x.status != RentalPropertyStatus.Inactive).CountAsync();
        }

        public async Task<int> CountAvailableUnits()
        {
            return await _dataContext.rentalProperties.Include(x => x.project)
                .Where(x => x.project.companyid == _currentUserService.companyid && x.status == RentalPropertyStatus.Vacant).CountAsync();
        }

        public async Task<int> CountActiveContracts()
        {
            return await _dataContext.rentalContracts
                .Where(x => x.companyid == _currentUserService.companyid && x.status == RentStatus.Active).CountAsync();
        }

        public async Task<FileData> GenerateSOA(long contractid)
        {
            var result = new FileData();
            var data = await GetContractById(contractid);
            result.filename = "soa-" + contractid + ".pdf";
            result.filepath = "wwwroot/temp/rental";
            string path = Path.Combine(Directory.GetCurrentDirectory(), result.filepath);
            result.url = $"{_configuration["BaseURL"]}/temp/rental/" + result.filename;
            string fullpath = path + "/" + result.filename;
            if (System.IO.File.Exists(fullpath))
                System.IO.File.Delete(fullpath);
            try
            {
                var company = await _dataContext.companies.FindAsync(data.companyid);
                var html = System.IO.File.ReadAllText(@"./Templates/Rental_SOA.html");
                var logoDisplay = "none";
                if (company.logo != "")
                    logoDisplay = "inline-block";

                var propertyList = await GetPropertiesAsString(contractid);
                var body = html.Replace("logo_display", logoDisplay)
                    .Replace("{logo_path}", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/attachments/" + company.id + "/" + company.logo))
                    .Replace("{companyname}", company.name)
                    .Replace("{companyaddress}", company.address)
                    .Replace("{companycontact}", company.contactno)
                    .Replace("{asofdate}", Utility.GetServerTime().ToString(Constants.dateFormat))
                    .Replace("{customername}", data.client.fname + " " + data.client.lname)
                    .Replace("{customeraddress}", data.client.address)
                    .Replace("{rentedproperty}", propertyList)
                    .Replace("{contractno}", data.contractno.ToString())
                    .Replace("{contractdate}", data.contractdate.ToString(Constants.dateFormat))
                    .Replace("{billingsched}", "Every " + Utility.GetNumberRank(data.billingsched))
                    .Replace("{monthlyrent}", data.montlyrent.ToString(Constants.moneyFormat));

                var details = await GetSOA(contractid);
                var payables = "";
                foreach (var item in details)
                {
                    payables += $"<tr style=\"font-size: 14px;\"><td style=\"padding: 5px;\">{item.description}</td><td class=\"text-right\">{item.grossAmount.ToString(Constants.moneyFormat)}</td><td class=\"text-right\">{item.balance.ToString(Constants.moneyFormat)}</td></tr>";
                }
                var totalPayables = details.Sum(x => x.balance);
                body = body.Replace("{details}", payables)
                    .Replace("{totalbalance}", totalPayables.ToString(Constants.moneyFormat));

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    PaperSize = PaperKind.A4,
                    DocumentTitle = "SOA-" + contractid,
                    Out = fullpath,
                },
                    Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = body,
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = @"./Templates/main.css" },
                    }
                }
                };
                _converter.Convert(doc);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> SendSOA(MailingInfo info)
        {
            var contract = await GetContractById(info.id);
            try
            {
                var data = await GenerateSOA(info.id);
                if (data == null)
                    return false;
                var path = Path.Combine(Directory.GetCurrentDirectory(), data.fullpath);
                var html = File.ReadAllText(@"./Templates/ClientSOAEmail.html");
                var body = html.Replace("{main_link}", _configuration["uiBaseURL"]).Replace("{customername}", contract.client.fname + " " + contract.client.lname);
                _mailService.SendEmailAsync("Statement Of Account", [info.email], body, [path], true);
                await _logService.SaveLogAsync(contract.companyid, _currentUserService.employeeid, AppModule.Rentals, "SOA", $"Sent SOA to {info.email}", 0);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}
