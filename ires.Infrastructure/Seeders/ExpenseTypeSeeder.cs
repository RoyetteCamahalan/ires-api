using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;

namespace ires.Infrastructure.Seeders
{
    public class ExpenseTypeSeeder
    {
        private readonly DataContext _dataContext;

        public ExpenseTypeSeeder(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task Seed(int companyID)
        {
            List<ExpenseType> data = new()
            {
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Maintenance And Repairs", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Professional Services", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Salaries and Wages", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Bank Fees", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Taxes and Licenses", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Office Expenses", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Office Supplies", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Utilities", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Depreciation", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Rent", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Travel", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 2, expensetypedesc = "Mortgage Interest", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Postage and Shipping", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Deductions & Contributions", companyid = companyID, isactive = true },
                new ExpenseType{ expensetypecat = 1, expensetypedesc = "Representation", companyid = companyID, isactive = true },
            };
            _dataContext.expenseTypes.AddRange(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
