using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;

namespace ires.Infrastructure.Repositories
{
    public class LogRepository : ILogService
    {
        private readonly DataContext _dataContext;

        public LogRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task SaveLogAsync(int companyID, long employeeID, AppModule moduleID, string title, string action, int withadmin)
        {
            Log log = new Log
            {
                companyid = companyID,
                employeeid = employeeID,
                moduleid = moduleID,
                logtitle = title,
                logAction = action,
                logdate = Utility.GetServerTime(),
                withadmin = withadmin
            };
            _dataContext.logs.Add(log);
            await _dataContext.SaveChangesAsync();
        }

        public async Task SaveLogAsync(ICurrentUserContext currentUserContext, AppModule moduleID, string title, string action, int withadmin)
        {
            Log log = new Log
            {
                companyid = currentUserContext.companyid,
                employeeid = currentUserContext.employeeid,
                moduleid = moduleID,
                logtitle = title,
                logAction = action,
                logdate = Utility.GetServerTime(),
                withadmin = withadmin
            };
            _dataContext.logs.Add(log);
            await _dataContext.SaveChangesAsync();
        }
    }
}
