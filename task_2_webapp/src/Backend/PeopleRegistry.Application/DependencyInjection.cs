
using Microsoft.Extensions.DependencyInjection;

namespace PeopleRegistry.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add application services here
            // e.g., services.AddScoped<IMyService, MyService>();

            return services;
        }
    }
}
