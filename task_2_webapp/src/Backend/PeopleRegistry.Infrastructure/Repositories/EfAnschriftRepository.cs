using Microsoft.EntityFrameworkCore;
using Backend.PeopleRegistry.Infrastructure.Persistence;
using Backend.PeopleRegistry.Domain.Anschrift;
using Backend.PeopleRegistry.Domain.Person;

namespace Backend.PeopleRegistry.Infrastructure.Repositories;

public class EfAnschriftRepository : IAnschriftRepository
{
    private readonly PeopleDbContext _db;
    public EfAnschriftRepository(PeopleDbContext db) => _db = db;

    public async Task<List<Anschrift>> GetByPersonIdAsync(Guid personId, CancellationToken ct = default) =>
        await _db.Anschriften
            .AsNoTracking()
            .Where(a => a.PersonId == personId)
            .ToListAsync(ct);


    public async Task<Anschrift?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.Anschriften
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task AddAsync(Anschrift address, CancellationToken ct = default)
    {
        _db.Anschriften.Add(address);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Anschrift address, CancellationToken ct = default)
    {
        // _db.Anschriften.Update(address);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Anschriften.FirstOrDefaultAsync(a => a.Id == id, ct);
        if (entity is null) return;
        _db.Anschriften.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }
    
}