using ires.Domain.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ires.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor _httpContextAccessor) : ICurrentUserService
    {
        public long employeeid => Convert.ToInt64(_httpContextAccessor?.HttpContext?.User?.Claims?.Single(x => x.Type == ClaimTypes.PrimarySid).Value);

        public int companyid => Convert.ToInt32(_httpContextAccessor?.HttpContext?.User?.Claims?.Single(x => x.Type == ClaimTypes.PrimaryGroupSid).Value);
    }
}
