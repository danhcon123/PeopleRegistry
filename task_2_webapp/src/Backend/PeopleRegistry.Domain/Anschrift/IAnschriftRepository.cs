namespace Backend.PeopleRegistry.Domain.Anschrift;

/// <summary>
/// Definiert die Schnittstelle für den Datenzugriff auf <see cref="Anschrift"/>-Entitäten.
/// Ermöglicht das Abrufen, Hinzufügen, Aktualisieren und Löschen von Adressen.
/// </summary>
public interface IAnschriftRepository
{
    /// <summary>
    /// Ruft alle Anschriften ab, die einer bestimmten Person zugeordnet sind.
    /// </summary>
    Task<List<Anschrift>> GetByPersonIdAsync(Guid personId, CancellationToken ct = default);

    /// <summary>
    /// Ruft eine einzelne Anschrift anhand ihrer eindeutigen ID ab.
    /// </summary>
    Task<Anschrift> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Fügt eine neue Anschrift in das Repository ein.
    /// </summary>
    Task AddAsync(Anschrift address, CancellationToken ct = default);

    /// <summary>
    /// Aktualisiert eine bestehende Anschrift.
    /// </summary>
    Task UpdateAsync(Anschrift address, CancellationToken ct = default);

    /// <summary>
    /// Löscht eine Anschrift anhand ihrer eindeutigen ID.
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}