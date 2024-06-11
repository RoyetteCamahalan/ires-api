using ires_api.Enumerations;

namespace ires_api.Services.Interface
{
    public interface ILogService
    {
        public void SaveLog(int companyID, long employeeID, AppModule moduleID, string title, string action, int withadmin);
    }
}
