using ires.Domain.DTO;

namespace ires.Domain.Contracts
{
    public interface IAppService
    {
        public Task<ICollection<NotificationViewModel>> GetNotifications(long employeeID);
        public Task MarkAsReadNotif(long id);
        public Task MarkAllAsReadNotif(long employeeID);
        public Task<ICollection<EventViewModel>> GetEvents(int companyID, DateTime startDate, DateTime endDate);

        public Task ReloadRentalContracts();
        public Task ReloadSubscriptions();
        public Task SendBillingAccountNotifications();
        public Task<bool> ExecuteJob(string job);
    }
}
