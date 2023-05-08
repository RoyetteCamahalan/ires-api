using ires_api.Data;
using ires_api.Models;

namespace ires_api.Services.Seeders
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
            List<Vendor> vendors = new()
            {
                new Vendor{ vendorname="N/A", companyid = companyID, isactive = true },
            };
            _dataContext.vendors.AddRange(vendors);
            await _dataContext.SaveChangesAsync();
        }
    }
}
