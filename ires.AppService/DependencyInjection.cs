using Microsoft.Extensions.DependencyInjection;

namespace ires.AppService
{
    public static class DependencyInjection
    {
        public static void AddAppService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        }
    }
}
