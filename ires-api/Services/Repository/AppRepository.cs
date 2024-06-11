using AutoMapper;
using ires_api.Data;
using ires_api.DTO;
using ires_api.DTO.Survey;
using ires_api.Enumerations;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
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

        public async Task<ICollection<Notification>> GetNotifications(long employeeID)
        {
            return await _dataContext.notifications.Where(x => x.employeeid == employeeID && !x.isread).OrderByDescending(x => x.datecreated).ToListAsync();
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
            return await _dataContext.surveys.Where(x => x.companyid == companyID && x.status != Constants.SurveyStatus.cancelled &&
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
