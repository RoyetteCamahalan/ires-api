using ires.Domain.DTO;

namespace ires.Domain.Contracts
{
    public interface IAppService
    {
        public Task<ICollection<NotificationViewModel>> GetNotifications(long employeeID);
        public Task MarkAsReadNotif(long id);
        public Task<ICollection<EventViewModel>> GetEvents(int companyID, DateTime startDate, DateTime endDate);
    }
}
