using ires.Domain.DTO;

namespace ires.Domain.Contracts
{
    public interface IAppService
    {
        public Task<ICollection<NotificationViewModel>> GetNotifications();
        public Task MarkAsReadNotif(long id);
        public Task MarkAllAsReadNotif();
        public Task<ICollection<EventViewModel>> GetEvents(DateTime startDate, DateTime endDate);

        public Task ReloadRentalContracts();
        public Task ReloadSubscriptions();
        public Task<bool> ExecuteJob(string job);
    }
}
