using ires.Domain.Enumerations;

namespace ires.Domain.Contracts
{
    public interface ILogService
    {
        public Task SaveLogAsync(int companyID, long employeeID, AppModule moduleID, string title, string action, int withadmin);
        public Task SaveLogAsync(ICurrentUserContext currentUserContext, AppModule moduleID, string title, string action, int withadmin);
    }
}
