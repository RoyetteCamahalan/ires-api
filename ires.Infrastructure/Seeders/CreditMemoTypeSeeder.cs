using ires.Domain;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Seeders
{
    internal class CreditMemoTypeSeeder
    {
        private readonly DataContext _dataContext;

        public CreditMemoTypeSeeder(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task Seed(int companyID)
        {
            if (await _dataContext.creditMemoTypes.Where(x => x.companyid == companyID).AnyAsync())
                return;
            var currentDateTime = Utility.GetServerTime();
            var data = new List<CreditMemoType> {
                new CreditMemoType{ name="Monthly Rebate", companyid = companyID, isactive = true, datecreated = currentDateTime },
                new CreditMemoType{ name="Penalty Discount", companyid = companyID, isactive = true, datecreated = currentDateTime },
            };
            _dataContext.AddRange(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
