using Backend.PeopleRegistry.Application.Services;
using Backend.PeopleRegistry.Application.Models;
using Backend.PeopleRegistry.Domain.Person;
using Backend.PeopleRegistry.Domain.Anschrift;
using Backend.PeopleRegistry.Domain.Telefonverbindung;

using Microsoft.EntityFrameworkCore;
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
    public async Task<Person?> GetDetailsAsync(Guid id, CancellationToken ct = default)
    {
        // Repo Methode liefert zurück
        
        return await _personRepo.GetByIdWithChildrenAsync(id, ct);
    }

    public async Task UpdateDetailsAsync(Guid id, string vorname, string nachname, DateTime? geburtsdatum,
                            IEnumerable<AddressModel> addresses,
                            IEnumerable<PhoneModel> phones,
                            CancellationToken ct)
    {
        // Akzeptiere leere Sequenzen statt null
        addresses ??= Array.Empty<AddressModel>();
        phones ??= Array.Empty<PhoneModel>();

        using var scope = _logger.BeginScope(new Dictionary<string, object> { ["PersonId"] = id });

        try
        {
            _logger.LogInformation("Begin UpdateDetails for Person {PersonId}", id);

            // 1) Aggregat mit Kindern laden (tracking ON: wir mutieren das Aggregat)
            var person = await _personRepo.GetByIdWithChildrenAsync(id, ct)
                        ?? throw new KeyNotFoundException("Person nicht gefunden");

            var beforeAddrCount = person.Anschriften.Count;
            var beforePhoneCount = person.Telefonverbindungen.Count;

            _logger.LogDebug("Loaded person with {AddressCount} addresses and {PhoneCount} phones",
                beforeAddrCount, beforePhoneCount);

            // 2) Basisdaten normalisieren/aktualisieren
            person.Vorname = (vorname ?? string.Empty).Trim();
            person.Nachname = (nachname ?? string.Empty).Trim();
            person.Geburtsdatum = geburtsdatum;

            _logger.LogDebug("Updating basic fields: Vorname='{Vorname}', Nachname='{Nachname}', Geburtsdatum={Geburtsdatum}",
                person.Vorname, person.Nachname, person.Geburtsdatum);

            // 3) Anschriften Upsert
            _logger.LogDebug("Incoming address DTO count: {IncomingCount}", addresses.Count());

            var addrById = person.Anschriften.ToDictionary(a => a.Id, a => a);
            var incomingAddrIds = new HashSet<Guid>();

            foreach (var dto in addresses)
            {
                var hasId = dto.Id.HasValue && dto.Id.Value != Guid.Empty;

                if (hasId && addrById.TryGetValue(dto.Id!.Value, out var existing))
                {
                    existing.Postleitzahl = dto.Postleitzahl;
                    existing.Ort = dto.Ort;
                    existing.Strasse = dto.Strasse;
                    existing.Hausnummer = dto.Hausnummer;

                    incomingAddrIds.Add(existing.Id); // Monitor den echten ID

                    _logger.LogTrace("Address update {AddressId}: {PLZ} {Ort} {Strasse} {Hausnummer}",
                        existing.Id, dto.Postleitzahl, dto.Ort, dto.Strasse, dto.Hausnummer);
                }
                else
                {
                    if (hasId && !addrById.ContainsKey(dto.Id!.Value))
                        _logger.LogWarning("Client sent unknown Address Id {Id}; creating new.", dto.Id!.Value);
                        
                    // Insert neuer Eintrag
                    var newAddr = new Anschrift(
                        id: Guid.NewGuid(),
                        personId: person.Id,
                        postleitzahl: dto.Postleitzahl,
                        ort: dto.Ort,
                        strasse: dto.Strasse,
                        hausnummer: dto.Hausnummer
                    );
                    person.Anschriften.Add(newAddr);

                    _logger.LogTrace("Address add {AddressId}: {PLZ} {Ort} {Strasse} {Hausnummer}",
                        newAddr.Id, dto.Postleitzahl, dto.Ort, dto.Strasse, dto.Hausnummer);

                    incomingAddrIds.Add(newAddr.Id);  // Monitor server-generierte Id
                }
            }

            // Delete: Alles, was nicht im Zielzustand ist, löschen
            var removedAddrCount = person.Anschriften.RemoveAll(a => !incomingAddrIds.Contains(a.Id));
            if (removedAddrCount > 0)
                _logger.LogDebug("Addresses removed: {RemovedCount}", removedAddrCount);

            // 4) Telefonverbindungen Upsert
            _logger.LogDebug("Incoming phone DTO count: {IncomingCount}", phones.Count());

            var phoneById = person.Telefonverbindungen.ToDictionary(p => p.Id, p => p);
            var incomingPhoneIds = new HashSet<Guid>();

            foreach (var dto in phones)
            {
                var hasId = dto.Id.HasValue && dto.Id.Value != Guid.Empty;

                if (hasId && phoneById.TryGetValue(dto.Id!.Value, out var existing))
                {
                    // Achtung PII: Nummer nicht auf Info-Level loggen. Trace-Only falls nötig.
                    existing.Telefonnummer = dto.Telefonnummer;

                    incomingPhoneIds.Add(existing.Id);
                    _logger.LogTrace("Phone update {PhoneId}", existing.Id);
                }
                else
                {
                    if (hasId && !phoneById.ContainsKey(dto.Id!.Value))
                        _logger.LogWarning("Client sent unknown Phone Id {Id}; creating new.", dto.Id!.Value);

                    var newPhone = new Telefonverbindung(
                        id: Guid.NewGuid(),
                        personId: person.Id,
                        telefonnummer: dto.Telefonnummer
                    );
                    person.Telefonverbindungen.Add(newPhone);
                    
                    incomingPhoneIds.Add(newPhone.Id);
                    _logger.LogTrace("Phone add {PhoneId}", newPhone.Id);
                }
            }

            var removedPhoneCount = person.Telefonverbindungen.RemoveAll(p => !incomingPhoneIds.Contains(p.Id));
            if (removedPhoneCount > 0)
                _logger.LogDebug("Phones removed: {RemovedCount}", removedPhoneCount);

            // 5) Persistieren (eine Transaktion/Save)
            await _personRepo.UpdateAsync(person, ct);

            _logger.LogInformation(
                "UpdateDetails completed: addresses {BeforeAddrCount}→{AfterAddrCount}, phones {BeforePhoneCount}→{AfterPhoneCount}",
                beforeAddrCount, person.Anschriften.Count, beforePhoneCount, person.Telefonverbindungen.Count);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(PersonLogEvents.GetById, ex, "Person not found");
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(PersonLogEvents.Update, ex, "Database update failed during person update");
            throw;
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation(PersonLogEvents.Update, "UpdateDetails operation was cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(PersonLogEvents.Update, ex, "Unexpected error during UpdateDetails");
            throw;
        }
    }
}
