namespace Backend.PeopleRegistry.Domain.Telefonverbindung;

/// <summary>
/// Definiert die Schnittstelle für den Zugriff auf Telefonnummer im Repository.
/// </summary>
public interface ITelefonverbindungRepository
{
    /// <summary>
    /// Ruft alle Personen asynchron ab.
    /// </summary>
    Task<List<Telefonverbindung>> GetAllByPersonIdAsync(CancellationToken ct = default);

    /// <summary>
    /// Fügt eine neue Person zum Repository hinzu.
    /// </summary>
    Task AddAsync(Telefonverbindung telefonverbindung, CancellationToken ct = default);

    /// <summary>
    /// Aktualisiert eine bestehende Person im Repository.
    /// </summary>
    Task UpdateAsync(Telefonverbindung telefonverbindung, CancellationToken ct = default);

    Task DeleteAsync(Telefonverbindung telefonverbindung, CancellationToken ct = default);
}