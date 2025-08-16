using ires.Domain.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ires.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor _httpContextAccessor) : ICurrentUserService
    {
        public long employeeid => Convert.ToInt64(_httpContextAccessor?.HttpContext?.User?.Claims?.Single(x => x.Type == ClaimTypes.PrimarySid).Value);

        public int companyid => Convert.ToInt32(_httpContextAccessor?.HttpContext?.User?.Claims?.Single(x => x.Type == ClaimTypes.PrimaryGroupSid).Value);
    }
}
