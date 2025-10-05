using Microsoft.EntityFrameworkCore;
using Backend.PeopleRegistry.Infrastructure.Persistence;
using Backend.PeopleRegistry.Domain.Telefonverbindung;

namespace Backend.PeopleRegistry.Infrastructure.Repositories;

public class EfTelefonverbindungRepository : ITelefonverbindungRepository
{
    private readonly PeopleDbContext _db;
    public EfTelefonverbindungRepository(PeopleDbContext db) => _db = db;
    public async Task<List<Telefonverbindung>> GetByPersonIdAsync(Guid personId, CancellationToken ct = default) =>
        await _db.Telefonverbindungs
        .AsNoTracking()
        .Where(t => t.PersonId == personId)
        .ToListAsync(ct);

    /// <summary>
    /// Ruf Telefonnummer ab, die die ID entspricht.
    /// </summary>
    public async Task<Telefonverbindung?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.Telefonverbindungs
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == id, ct);

    /// <summary>
    /// Fügt eine neue Telefonnummer dem Repository hinzu.
    /// </summary>
    public async Task AddAsync(Telefonverbindung phone, CancellationToken ct = default)
    {
        _db.Telefonverbindungs.Add(phone);
        await _db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Aktualisiert eine bestehende Telefonnummer im Repository.
    /// </summary>
    public async Task UpdateAsync(Telefonverbindung phone, CancellationToken ct = default)
    {
        // _db.Telefonverbindungs.Update(phone);
        await _db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Löscht eine bestehende Telefonnummer aus dem Repository.
    /// </summary>
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Telefonverbindungs.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (entity is null) return;
        _db.Telefonverbindungs.Remove(entity);
    }
}