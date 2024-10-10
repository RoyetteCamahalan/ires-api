using ires.Domain.Enumerations;

namespace ires.Domain.Contracts
{
    public interface ILogService
    {
        public Task SaveLogAsync(AppModule moduleID, string title, string action, int withadmin = 0);
        public Task SaveLogAsync(AppModule moduleID, string title, dynamic data, int withadmin = 0);
        public Task SaveLogAsync(int companyID, long employeeID, AppModule moduleID, string title, string action, int withadmin);
    }
}
