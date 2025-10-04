using Backend.PeopleRegistry.Application.Services;
using Backend.PeopleRegistry.Domain.Person;

using Microsoft.Extensions.Logging;

namespace Backend.PeopleRegistry.Infrastructure.Services;

internal static class PersonLogEvents
{
    public static readonly EventId GetAll = new(1000, nameof(GetAll));
    public static readonly EventId SearchByName = new(1001, nameof(SearchByName));
    public static readonly EventId GetById = new(1002, nameof(GetById));
    public static readonly EventId Create = new(1003, nameof(Create));
    public static readonly EventId Update = new(1004, nameof(Update));
    public static readonly EventId Validation = new(1401, "ValidationError");
    public static readonly EventId Duplicate = new(1402, "DuplicatePerson");
}

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepo;
    private readonly ILogger<PersonService> _logger;
    public PersonService(IPersonRepository personRepo, ILogger<PersonService> logger)
    {
        _personRepo = personRepo;
        _logger = logger;
    }

    public async Task<List<Person>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug(PersonLogEvents.GetAll, "Fetching all persons…");
        var list = await _personRepo.GetAllAsync(ct);
        _logger.LogInformation(PersonLogEvents.GetAll, "Fetched {Count} persons.", list.Count);
        return list;
    }

    public async Task<List<Person>> SearchByName(string name, CancellationToken ct = default)
    {
        var term = (name ?? string.Empty).Trim();
        _logger.LogDebug(PersonLogEvents.SearchByName, "Searching persons by name contains: '{Term}'", term);
        var list = await _personRepo.SearchByNameAsync(term, ct);
        _logger.LogInformation(PersonLogEvents.SearchByName, "Search '{Term}' returned {Count} persons.", term, list.Count);
        return list;
    }

    public async Task<Person?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object> { ["PersonId"] = id });
        _logger.LogDebug(PersonLogEvents.GetById, "Fetching person by id…");

        var person = await _personRepo.GetByIdAsync(id, ct);

        if (person is null)
            _logger.LogWarning(PersonLogEvents.GetById, "Person not found.");

        return person;
    }

    public async Task<Person> CreateAsync(string vorname, string nachname, DateTime? geburtsdatum, CancellationToken ct)
    {
        vorname = (vorname ?? "").Trim();
        nachname = (nachname ?? "").Trim();

        _logger.LogDebug(PersonLogEvents.Create, "Creating person: {Vorname} {Nachname} ({Geburtsdatum})",
            vorname, nachname, geburtsdatum);

        if (string.IsNullOrWhiteSpace(vorname) || string.IsNullOrWhiteSpace(nachname))
        {
            _logger.LogWarning(PersonLogEvents.Validation, "Validation failed: Vorname/Nachname required.");
            throw new ArgumentException("Vorname und Nachname sind erforderlich.");
        }

        if (await _personRepo.ExistsAsync(vorname, nachname, geburtsdatum, ct))
        {
            _logger.LogWarning(PersonLogEvents.Duplicate, "Duplicate detected for {Vorname} {Nachname} ({Geburtsdatum}).",
                vorname, nachname, geburtsdatum);
            throw new InvalidOperationException("Person existiert bereits.");
        }

        var p = new Person(
            id: Guid.NewGuid(),
            vorname: vorname,
            nachname: nachname,
            geburtsdatum: geburtsdatum ?? DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc) // placeholder
        );

        await _personRepo.AddAsync(p, ct);

        using var scope = _logger.BeginScope(new Dictionary<string, object> { ["PersonId"] = p.Id });
        _logger.LogInformation(PersonLogEvents.Create, "Person created successfully.");
        return p;
    }

    public async Task UpdateAsync(Guid id, string vorname, string nachname, DateTime? geburtsdatum, CancellationToken ct)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object> { ["PersonId"] = id });
        _logger.LogDebug(PersonLogEvents.Update, "Updating person…");

        var person = await _personRepo.GetByIdAsync(id, ct);
        if (person is null)
        {
            _logger.LogWarning(PersonLogEvents.Update, "Person not found.");
            throw new KeyNotFoundException("Person nicht gefunden.");
        }

        person.Vorname = (vorname ?? "").Trim();
        person.Nachname = (nachname ?? "").Trim();
        person.Geburtsdatum = geburtsdatum;

        await _personRepo.UpdateAsync(person, ct);
        _logger.LogInformation(PersonLogEvents.Update, "Person updated successfully.");
    }
}
