using ires_api.Data;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
{
    public class AppRepository : IAppService
    {
        private readonly DataContext _dataContext;

        public AppRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<ICollection<Notification>> GetNotifications(long employeeID)
        {
            return await _dataContext.notifications.Where(x => x.employeeid == employeeID && !x.isread).OrderByDescending(x => x.datecreated).ToListAsync();
        }

        public async Task MarkAsReadNotif(long id)
        {
            var notif = await _dataContext.notifications.FindAsync(id);
            if (notif != null && !notif.isread)
            {
                notif.isread = true;
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
