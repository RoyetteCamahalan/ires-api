using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;

namespace ires.Infrastructure.Repositories
{
    public class LogRepository(DataContext _dataContext, ICurrentUserService _currentUserService) : ILogService
    {

        public async Task SaveLogAsync(AppModule moduleID, string title, string action, int withadmin = 0)
        {
            await SaveLogAsync(_currentUserService.companyid, _currentUserService.employeeid, moduleID, title, action, withadmin);
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
    }
}
