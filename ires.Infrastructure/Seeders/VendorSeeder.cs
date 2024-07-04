using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Seeders
{
    public class VendorSeeder
    {
        private readonly DataContext _dataContext;

        public VendorSeeder(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Seed(int companyID)
        {
            if (await _dataContext.vendors.Where(x => x.companyid == companyID).AnyAsync())
                return;
            List<Vendor> vendors = new()
            {
                new Vendor{ vendorname="N/A", companyid = companyID, isactive = true },
            };
            _dataContext.vendors.AddRange(vendors);
            await _dataContext.SaveChangesAsync();
        }
    }
}
