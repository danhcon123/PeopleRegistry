namespace Backend.PeopleRegistry.Domain.Person;


/// <summary>
/// Definiert die Schnittstelle für den Zugriff auf Personenobjekte im Repository.
/// </summary>
public interface IPersonRepository
{
    /// <summary>
    /// Ruft alle Personen asynchron ab.
    /// </summary>
    Task<List<Person>> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Sucht Personen anhand eines Namens oder Namensausschnitts.
    /// </summary>
    Task<List<Person>> SearchByNameAsync(string name, CancellationToken ct = default);

    /// <summary>
    /// Ruft eine bestimmte Person anhand ihrer eindeutigen ID ab.
    /// </summary>
    Task<Person?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Fügt eine neue Person zum Repository hinzu.
    /// </summary>
    Task AddAsync(Person person, CancellationToken ct = default);

    /// <summary>
    /// Aktualisiert eine bestehende Person im Repository.
    /// </summary>
    Task UpdateAsync(Person person, CancellationToken ct = default);

    /// <summary>
    /// Prüfe nach Existierung
    /// </summary>
    Task<bool> ExistsAsync(string vorname, string nachname, DateTime? geburtsdatum, CancellationToken ct = default);
}