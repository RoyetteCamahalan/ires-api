using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;

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
            var data = new List<CreditMemoType> {
                new CreditMemoType{ name="Monthly Rebate", companyid = companyID, isactive = true, datecreated = DateTime.Now },
                new CreditMemoType{ name="Penalty Discount", companyid = companyID, isactive = true, datecreated = DateTime.Now },
            };
            _dataContext.AddRange(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
