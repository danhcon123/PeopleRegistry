using Microsoft.EntityFrameworkCore;
using Backend.PeopleRegistry.Infrastructure.Persistence;
using Backend.PeopleRegistry.Domain.Person;
using Backend.PeopleRegistry.Domain.Anschrift;
using Backend.PeopleRegistry.Domain.Telefonverbindung;


namespace Backend.PeopleRegistry.Infrastructure.Repositories;

public class EfPersonRepository : IPersonRepository
{
    private readonly PeopleDbContext _db;
    public EfPersonRepository(PeopleDbContext db) => _db = db;

    public async Task<List<Person>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Personen.AsNoTracking().ToListAsync(ct);

    public async Task<List<Person>> SearchByNameAsync(string name, CancellationToken ct = default) =>
        await _db.Personen.AsNoTracking()
            .Where(p => p.Vorname.Contains(name) || p.Nachname.Contains(name))
            .ToListAsync(ct);

    public async Task<Person?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.Personen.Include(p => p.Anschriften)
                          .Include(p => p.Telefonverbindungen)
                          .FirstOrDefaultAsync(p => p.Id == id, ct);
    public async Task<Person?> GetByIdWithChildrenAsync(Guid id, CancellationToken ct = default) =>
        await _db.Personen
            .Include(p => p.Anschriften)
            .Include(p => p.Telefonverbindungen)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task AddAsync(Person person, CancellationToken ct = default)
    {
        _db.Personen.Add(person);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Person person, CancellationToken ct = default)
    {
        Console.WriteLine("----- EF Change Tracker States BEFORE SaveChangesAsync -----");

        foreach (var entry in _db.ChangeTracker.Entries())
        {
            if (entry.Entity is Person || entry.Entity is Anschrift || entry.Entity is Telefonverbindung)
            {
                var entityType = entry.Entity.GetType().Name;
                var entityId = (entry.Entity as dynamic).Id;
                var state = entry.State;
                Console.WriteLine($"Entity: {entityType} | ID: {entityId} | State: {state}");
            }
        }
        Console.WriteLine("------------------------------------------------------------");

        await _db.SaveChangesAsync(ct);
        Console.WriteLine("✅ EF SaveChangesAsync completed");
    }

    public async Task<bool> ExistsAsync(string vorname, string nachname, DateTime? geburtsdatum, CancellationToken ct = default)
    {
        return await _db.Personen
                .AnyAsync(p =>
                    p.Vorname == vorname &&
                    p.Nachname == nachname &&
                    p.Geburtsdatum == geburtsdatum,
                    ct);
    }

    public void RemoveTelefonverbindung(Backend.PeopleRegistry.Domain.Telefonverbindung.Telefonverbindung telefon)
    {
        _db.Telefonverbindungs.Remove(telefon);
    }
    public void RemoveAnschrift(Backend.PeopleRegistry.Domain.Anschrift.Anschrift anschrift)
    {
        _db.Anschriften.Remove(anschrift);
    }

    public void AddTelefonverbindung(Backend.PeopleRegistry.Domain.Telefonverbindung.Telefonverbindung tel)
        => _db.Telefonverbindungs.Add(tel);

    public void AddAnschrift(Backend.PeopleRegistry.Domain.Anschrift.Anschrift anschrift)
        => _db.Anschriften.Add(anschrift);
}