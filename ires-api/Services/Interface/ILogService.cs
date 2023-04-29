namespace ires_api.Services.Interface
{
    public interface ILogService
    {
        public void SaveLog(int companyID, long employeeID, int moduleID, string title, string action, int withadmin);
    }
}
