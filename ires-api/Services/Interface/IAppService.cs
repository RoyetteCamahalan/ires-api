using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IAppService
    {
        public Task<ICollection<Notification>> GetNotifications(long employeeID);
        public Task MarkAsReadNotif(long id);
        public Task<ICollection<EventDto>> GetEvents(int companyID, DateTime startDate, DateTime endDate);
    }
}
