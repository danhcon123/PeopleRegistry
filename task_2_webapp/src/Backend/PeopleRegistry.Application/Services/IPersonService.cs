using Backend.PeopleRegistry.Domain.Person;
namespace Backend.PeopleRegistry.Application.Services;

/// <summary>
/// Definiert die Anwendungslogik für die Verwaltung von Personen.
/// Stellt Use-Case-orientierte Methoden bereit, die über einfache Datenzugriffe hinausgehen.
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
    /// Erstellt eine neue Person anhand der angegebenen Daten.
    /// Diese Methode wird verwendet, wenn keine ID vorgegeben wird (z. B. bei neuen Einträgen).
    /// </summary>
    Task<Person> CreateAsync(string vorname, string nachname, DateTime? geburtsdatum, CancellationToken ct);

    /// <summary>
    /// Aktuellisiere Daten einer Person mit einer explizit angegebenen ID.
    /// </summary>
    Task UpdateAsync(Guid id, string vorname, string nachname, DateTime? geburtsdatum, CancellationToken ct = default);
}