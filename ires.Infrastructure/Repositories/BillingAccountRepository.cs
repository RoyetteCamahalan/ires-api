using AutoMapper;
using ires.Domain.Contracts;
using ires.Domain.DTO.BillingAccount;
using ires.Domain.DTO.BillingPayment;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class BillingAccountRepository(DataContext dataContext, IAccountService accountService, IExpenseService expenseService, IMapper mapper, ICurrentUserContext currentUserContext, ILogService logService) : IBillingAccountService
    {

        #region "Billing Accounts"

        public async Task<BillingAccountViewModel> CreateBillingAccount(BillingAccountRequestDto requestDto)
        {
            var entity = mapper.Map<BillingAccount>(requestDto);
            entity.Id = 0;
            entity.IsActive = true;
            entity.CompanyId = currentUserContext.companyid;
            entity.CreatedById = currentUserContext.employeeid;
            entity.DateCreated = DateTime.UtcNow;
            if (entity.Frequency == BillingFrequency.Monthly && entity.DueDayOfMonth.HasValue)
            {
                entity.NextDueDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, entity.DueDayOfMonth.Value);
                if (entity.NextDueDate < DateTime.UtcNow)
                {
                    entity.NextDueDate = entity.NextDueDate.Value.AddMonths(1);
                }
            }
            else if (entity.Frequency == BillingFrequency.Weekly && entity.DueDayOfWeek.HasValue)
            {
                var daysUntilNextDue = ((int)entity.DueDayOfWeek.Value - (int)DateTime.UtcNow.DayOfWeek + 7) % 7;
                entity.NextDueDate = DateTime.UtcNow.AddDays(daysUntilNextDue);
            }

            dataContext.billingAccounts.Add(entity);
            await dataContext.SaveChangesAsync();
            await logService.SaveLogAsync(currentUserContext, AppModule.Billing, "BillingAccount", "Create Billing Account: " + entity.Id + "-" + entity.AccountName, 0);
            return mapper.Map<BillingAccountViewModel>(await GetBillingAccountEntityById(entity.Id));
        }

        public async Task<bool> UpdateBillingAccount(BillingAccountRequestDto requestDto)
        {
            var entity = await dataContext.billingAccounts.FirstOrDefaultAsync(x => x.Id == requestDto.Id);
            if (entity == null) return false;

            entity.AccountName = requestDto.AccountName;
            entity.VendorId = requestDto.VendorId;
            entity.OfficeId = requestDto.OfficeId;
            entity.ExpenseTypeId = requestDto.ExpenseTypeId;
            entity.HasFixedAmount = requestDto.HasFixedAmount;
            entity.Amount = requestDto.HasFixedAmount ? requestDto.Amount : null;
            entity.Frequency = requestDto.Frequency;
            entity.DueDayOfMonth = requestDto.DueDayOfMonth;
            entity.DueDayOfWeek = requestDto.DueDayOfWeek;
            entity.NotifyDaysBefore = requestDto.NotifyDaysBefore;
            entity.NotifyOption = requestDto.NotifyOption;
            entity.Memo = requestDto.Memo;
            entity.UpdatedById = currentUserContext.employeeid;
            entity.DateUpdated = DateTime.UtcNow;
            // Recalculate next due date if frequency or due day changed, based on current next due date ?? or current date if next due date is null
            if (entity.Frequency == BillingFrequency.Monthly && entity.DueDayOfMonth.HasValue)
            {
                var baseDate = entity.NextDueDate ?? DateTime.UtcNow;
                var nextDue = new DateTime(baseDate.Year, baseDate.Month, entity.DueDayOfMonth.Value);
                if (nextDue < baseDate) nextDue = nextDue.AddMonths(1);
                entity.NextDueDate = nextDue;
            }
            else if (entity.Frequency == BillingFrequency.Weekly && entity.DueDayOfWeek.HasValue)
            {
                var baseDate = entity.NextDueDate ?? DateTime.UtcNow;
                var daysUntilNextDue = ((int)entity.DueDayOfWeek.Value - (int)baseDate.DayOfWeek + 7) % 7;
                entity.NextDueDate = baseDate.AddDays(daysUntilNextDue);
            }

            await dataContext.SaveChangesAsync();
            await logService.SaveLogAsync(entity.CompanyId, entity.UpdatedById, AppModule.Billing, "BillingAccount", "Update Billing Account: " + entity.Id + "-" + entity.AccountName, 0);
            return true;
        }

        public async Task<BillingAccountViewModel?> GetBillingAccountById(long id)
        {
            return mapper.Map<BillingAccountViewModel>(await GetBillingAccountEntityById(id));
        }

        public async Task<ICollection<BillingAccountViewModel>> GetBillingAccounts(bool viewAll, string search)
        {
            var query = dataContext.billingAccounts
                .Include(x => x.Vendor)
                .Include(x => x.Office)
                .Include(x => x.ExpenseType)
                .Where(x => x.IsActive || viewAll);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => x.AccountName.Contains(search) || x.Vendor!.vendorname.Contains(search));

            var result = await query.OrderBy(x => x.AccountName).ToListAsync();
            return mapper.Map<ICollection<BillingAccountViewModel>>(result);
        }

        public async Task<bool> SetBillingAccountStatus(long id, bool isActive)
        {
            var entity = await dataContext.billingAccounts.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return false;

            entity.IsActive = isActive;
            entity.UpdatedById = currentUserContext.employeeid;
            entity.DateUpdated = DateTime.UtcNow;
            await dataContext.SaveChangesAsync();
            await logService.SaveLogAsync(currentUserContext, AppModule.Billing, "BillingAccount", $"{(isActive ? "Activate" : "Deactivate")} Billing Account: {entity.Id}-{entity.AccountName}", 0);
            return true;
        }

        private async Task<BillingAccount?> GetBillingAccountEntityById(long id)
        {
            return await dataContext.billingAccounts
                .Include(x => x.Vendor)
                .Include(x => x.Office)
                .Include(x => x.ExpenseType)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        #endregion

        #region "Billing Payments"

        public async Task<BillingPaymentViewModel> PostPayment(BillingPaymentRequestDto requestDto)
        {
            var account = await dataContext.billingAccounts
                .Include(x => x.ExpenseType)
                .FirstOrDefaultAsync(x => x.Id == requestDto.BillingAccountId);

            if (account == null) throw new InvalidOperationException("Billing account not found.");

            // Insert linked expense record
            var expense = new Expense
            {
                expenseid = 0,
                companyid = currentUserContext.companyid,
                transno = (await dataContext.expenses.MaxAsync(x => (long?)x.transno) ?? 0) + 1,
                accountid = account.OfficeId,
                expensetypeid = account.ExpenseTypeId,
                refno = requestDto.RefNo,
                refdate = requestDto.PaymentDate,
                amount = requestDto.Amount,
                memo = $"Billing: {account.AccountName} | Period: {requestDto.PeriodFrom:yyyy-MM-dd} - {requestDto.PeriodTo:yyyy-MM-dd}",
                transdate = DateTime.UtcNow,
                status = ExpenseStatus.approved,
                payeeid = account.VendorId,
                usepettycash = requestDto.UsePettyCash ?? false,
                createdbyid = currentUserContext.employeeid,
                updatedbyid = currentUserContext.employeeid
            };
            dataContext.expenses.Add(expense);
            await dataContext.SaveChangesAsync();

            var payment = mapper.Map<BillingPayment>(requestDto);
            payment.Id = 0;
            payment.ExpenseId = expense.expenseid;
            payment.Status = BillingPaymentStatus.Paid;
            payment.CompanyId = currentUserContext.companyid;
            payment.CreatedById = currentUserContext.employeeid;
            payment.DateCreated = DateTime.UtcNow;
            dataContext.billingPayments.Add(payment);

            account.NextDueDate = requestDto.NextDueDate;
            await dataContext.SaveChangesAsync();


            if (requestDto.UsePettyCash ?? false)
                await accountService.UpdateOfficeBalanceAsync(account.OfficeId, requestDto.Amount * -1);

            await expenseService.ReComputeAPAsync(account.VendorId);

            await logService.SaveLogAsync(currentUserContext, AppModule.Billing, "BillingPayment", "Post Payment for Account: " + account.AccountName, 0);
            return mapper.Map<BillingPaymentViewModel>(await GetBillingPaymentEntityById(payment.Id));
        }

        public async Task<bool> VoidPayment(long billingPaymentId)
        {
            var entity = await dataContext.billingPayments.FirstOrDefaultAsync(x => x.Id == billingPaymentId);
            if (entity == null) return false;

            entity.Status = BillingPaymentStatus.Voided;
            entity.UpdatedById = currentUserContext.employeeid;
            entity.DateUpdated = DateTime.UtcNow;

            if (entity.ExpenseId.HasValue)
            {
                var expense = await dataContext.expenses.FirstOrDefaultAsync(x => x.expenseid == entity.ExpenseId.Value);
                if (expense != null)
                {
                    expense.status = ExpenseStatus.@void;
                    expense.updatedbyid = currentUserContext.employeeid;
                    expense.dateupdated = DateTime.UtcNow;


                    var account = await dataContext.billingAccounts.FirstOrDefaultAsync(x => x.Id == entity.BillingAccountId);
                    if (account != null)
                    {
                        if (expense.usepettycash)
                        {
                            await accountService.UpdateOfficeBalanceAsync(account.OfficeId, expense.amount);
                        }

                        await expenseService.ReComputeAPAsync(account.VendorId);
                    }
                }
            }

            await dataContext.SaveChangesAsync();
            await logService.SaveLogAsync(currentUserContext, AppModule.Billing, "BillingPayment", "Void Payment: " + entity.Id, 0);
            return true;
        }

        public async Task<BillingPaymentViewModel?> GetBillingPaymentById(long id)
        {
            return mapper.Map<BillingPaymentViewModel>(await GetBillingPaymentEntityById(id));
        }

        public async Task<ICollection<BillingPaymentViewModel>> GetBillingPayments(long billingAccountId)
        {
            var result = await dataContext.billingPayments
                .Include(x => x.BillingAccount).ThenInclude(a => a!.Vendor)
                .Include(x => x.BillingAccount).ThenInclude(a => a!.Office)
                .Include(x => x.BillingAccount).ThenInclude(a => a!.ExpenseType)
                .Include(x => x.Expense)
                .Where(x => x.BillingAccountId == billingAccountId)
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();
            return mapper.Map<ICollection<BillingPaymentViewModel>>(result);
        }

        private async Task<BillingPayment?> GetBillingPaymentEntityById(long id)
        {
            return await dataContext.billingPayments
                .Include(x => x.BillingAccount).ThenInclude(a => a!.Vendor)
                .Include(x => x.BillingAccount).ThenInclude(a => a!.Office)
                .Include(x => x.BillingAccount).ThenInclude(a => a!.ExpenseType)
                .Include(x => x.Expense)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        #endregion
    }
}
