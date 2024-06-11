using ires_api.Data;
using ires_api.Enumerations;
using ires_api.Models;
using ires_api.Services.Interface;

namespace ires_api.Services.Repository
{
    public class LogRepository : ILogService
    {
        private readonly DataContext _dataContext;

        public LogRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void SaveLog(int companyID, long employeeID, AppModule moduleID, string title, string action, int withadmin)
        {
            Log log = new Log
            {
                companyid = companyID,
                employeeid = employeeID,
                moduleid = moduleID,
                logtitle = title,
                logAction = action,
                logdate = DateTime.Now,
                withadmin = withadmin
            };
            _dataContext.logs.Add(log);
            _dataContext.SaveChanges();
        }
    }
}
