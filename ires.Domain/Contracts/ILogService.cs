using ires.Domain.Enumerations;

namespace ires.Domain.Contracts
{
    public interface ILogService
    {
        public void SaveLog(int companyID, long employeeID, AppModule moduleID, string title, string action, int withadmin);
    }
}
