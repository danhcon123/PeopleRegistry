using Microsoft.EntityFrameworkCore;
using Backend.PeopleRegistry.Infrastructure.Persistence;
using Backend.PeopleRegistry.Domain.Person;

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

    public async Task AddAsync(Person person, CancellationToken ct = default)
    {
        _db.Personen.Add(person);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Person person, CancellationToken ct = default)
    {
        _db.Personen.Update(person);
        await _db.SaveChangesAsync(ct);
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
}