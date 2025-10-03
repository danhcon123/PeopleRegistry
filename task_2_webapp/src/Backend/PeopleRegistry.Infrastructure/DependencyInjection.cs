
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using Backend.PeopleRegistry.Infrastructure.Persistence;

namespace PeopleRegistry.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<PeopleDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));


        // Add other infrastructure services here
        // e.g., services.AddScoped<IMyRepository, MyRepository>();

        return services;
    }
}

