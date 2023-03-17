using ires_api.DTO;
using System.Security.Claims;

namespace ires_api
{
    public class IdentityProfile
    {
        public static UserLoginDto getIdentity(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new UserLoginDto
                {
                    employeeid = Convert.ToInt64(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value),
                    companyid = Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimaryGroupSid)?.Value)
                };
            }
            return null;
        }
    }
}
