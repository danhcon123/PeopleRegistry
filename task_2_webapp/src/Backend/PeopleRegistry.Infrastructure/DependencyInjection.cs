
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using Backend.PeopleRegistry.Domain.Person;
using Backend.PeopleRegistry.Domain.Anschrift;
using Backend.PeopleRegistry.Domain.Telefonverbindung;
using Backend.PeopleRegistry.Application.Services;
using Backend.PeopleRegistry.Infrastructure.Repositories;
using Backend.PeopleRegistry.Infrastructure.Persistence;
using Backend.PeopleRegistry.Infrastructure.Services;

namespace PeopleRegistry.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registriert alle Infrastruktur-Dienste (z. B. Datenbank, Repositories) für Dependency Injection.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext konfigurieren
        services.AddDbContext<PeopleDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        // Repository registrieren
        services.AddScoped<IPersonRepository, EfPersonRepository>();
        services.AddScoped<IAnschriftRepository, EfAnschriftRepository>();
        services.AddScoped<ITelefonverbindungRepository, EfTelefonverbindungRepository>();

        // Service registrieren
        services.AddScoped<IPersonService, PersonService>();

        return services;
    }
}

