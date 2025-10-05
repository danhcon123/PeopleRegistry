using Backend.PeopleRegistry.Domain.Person;
using Backend.PeopleRegistry.Application.Models;

namespace Backend.PeopleRegistry.Application.Services;

/// <summary>
/// Definiert die Anwendungslogik (Use-Case-Ebene) für die Verwaltung von Personen.
/// Bietet fachliche Operationen, die über einfache CRUD-Zugriffe hinausgehen.
/// </summary>
public interface IPersonService
{
    /// <summary>
    /// Ruft alle Personen asynchron ab.
    /// </summary>    
    Task<List<Person>> GetAllAsync(CancellationToken ct = default);


    /// <summary>
    /// Sucht Personen anhand eines Namens oder Namensausschnitts.
    /// </summary>
    Task<List<Person>> SearchByName(string name, CancellationToken ct = default);

    /// <summary>
    /// Ruft eine bestimmte Person anhand ihrer eindeutigen ID ab.
    /// </summary>
    Task<Person?> GetByIdAsync(Guid id, CancellationToken ct = default);

    // Use-case orientiert

    /// <summary>
    /// Erstellt eine neue Person anhand der angegebenen Daten (ohne vorgegebene ID).
    /// </summary>
    Task<Person> CreateAsync(string vorname, string nachname, DateTime? geburtsdatum, CancellationToken ct);

    /// <summary>
    /// Lädt eine Person inkl. abhängiger Daten (z. B. Anschriften, Telefonverbindungen).
    /// </summary>
    Task<Person?> GetDetailsAsync(Guid id, CancellationToken ct = default); // loads with children

    /// <summary>
    /// Aktualisiert die Stammdaten sowie die abhängigen Detaildaten (Anschriften, Telefonverbindungen) einer Person.
    /// Führt ein Upsert pro Kindliste durch (Update existierender Einträge, Insert neuer Einträge, Delete nicht mehr enthaltener Einträge).
    /// </summary>
    Task UpdateDetailsAsync(Guid id, string vorname, string nachname, DateTime? geburtsdatum,
                            IEnumerable<AddressModel> addresses,
                            IEnumerable<PhoneModel> phones,
                            CancellationToken ct);
}