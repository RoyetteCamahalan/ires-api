using AutoMapper;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Survey;
using ires.Domain.Enumerations;
using ires.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class AppRepository : IAppService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public AppRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ICollection<NotificationViewModel>> GetNotifications(long employeeID)
        {
            var result = await _dataContext.notifications.Where(x => x.employeeid == employeeID && !x.isread).OrderByDescending(x => x.datecreated).ToListAsync();
            return _mapper.Map<ICollection<NotificationViewModel>>(result);
        }

        public async Task MarkAsReadNotif(long id)
        {
            var notif = await _dataContext.notifications.FindAsync(id);
            if (notif != null && !notif.isread)
            {
                notif.isread = true;
                await _dataContext.SaveChangesAsync();
            }
        }
        public async Task<ICollection<EventViewModel>> GetEvents(int companyID, DateTime startDate, DateTime endDate)
        {
            return await _dataContext.surveys.Where(x => x.companyid == companyID && x.status != SurveyStatus.cancelled &&
                (x.surveydate ?? DateTime.MaxValue) >= startDate && (x.surveydate ?? DateTime.MaxValue) <= endDate)
                .Select(x => new EventViewModel
                {
                    id = x.id,
                    moduleid = AppModule.Surveying,
                    title = "Survey at " + x.propertyname,
                    date = x.surveydate ?? DateTime.MaxValue,
                    survey = _mapper.Map<SurveyViewModel>(x),
                }).ToListAsync();
        }


    }
}
