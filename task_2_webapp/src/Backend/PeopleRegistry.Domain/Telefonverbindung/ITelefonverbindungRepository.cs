namespace Backend.PeopleRegistry.Domain.Telefonverbindung;

/// <summary>
/// Definiert die Schnittstelle für den Datenzugriff auf <see cref="Telefonverbindung"/>-Entitäten.
/// Ermöglicht das Abrufen, Hinzufügen, Aktualisieren und Löschen von Telefonnummern.
/// </summary>
public interface ITelefonverbindungRepository
{
    /// <summary>
    /// Ruft alle Telefonnummern ab, die einer bestimmten Person zugeordnet sind. alle Personen asynchron ab.
    /// </summary>
    Task<List<Telefonverbindung>> GetByPersonIdAsync(Guid personId, CancellationToken ct = default);

    /// <summary>
    /// Ruf Telefonnummer ab, die die ID entspricht.
    /// </summary>
    Task<Telefonverbindung?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Fügt eine neue Telefonnummer dem Repository hinzu.
    /// </summary>
    Task AddAsync(Telefonverbindung phone, CancellationToken ct = default);

    /// <summary>
    /// Aktualisiert eine bestehende Telefonnummer im Repository.
    /// </summary>
    Task UpdateAsync(Telefonverbindung phone, CancellationToken ct = default);

    /// <summary>
    /// Löscht eine bestehende Telefonnummer aus dem Repository.
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}