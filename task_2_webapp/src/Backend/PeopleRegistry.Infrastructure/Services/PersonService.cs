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
        addresses ??= Array.Empty<AddressModel>();
        phones ??= Array.Empty<PhoneModel>();

        using var scope = _logger.BeginScope(new Dictionary<string, object> { ["PersonId"] = id });

        try
        {
            _logger.LogInformation("Begin UpdateDetails for Person {PersonId}", id);

            // 1) Load person with children
            var person = await _personRepo.GetByIdAsync(id, ct)
                        ?? throw new KeyNotFoundException("Person nicht gefunden");

            // 2) Detach all child entities from change tracker
            _personRepo.DetachAllChildren();

            // 2) Get existing collections
            var existingAddresses = person.Anschriften.ToList();
            var existingPhones = person.Telefonverbindungen.ToList();

            // 3) Update basic fields
            person.Vorname = (vorname ?? string.Empty).Trim();
            person.Nachname = (nachname ?? string.Empty).Trim();
            person.Geburtsdatum = geburtsdatum;

            // 4) Process Addresses
            var incomingAddressIds = addresses
                .Where(a => a.Id.HasValue && a.Id.Value != Guid.Empty)
                .Select(a => a.Id!.Value)
                .ToHashSet();

            // Delete addresses not in incoming list
            foreach (var addr in existingAddresses.Where(a => !incomingAddressIds.Contains(a.Id)))
            {
                person.Anschriften.Remove(addr);
                _personRepo.RemoveAnschrift(addr);
            }

            // Update or Add addresses
            foreach (var dto in addresses)
            {
                if (dto.Id.HasValue && dto.Id.Value != Guid.Empty)
                {
                    // Try to update existing
                    var existing = person.Anschriften.FirstOrDefault(a => a.Id == dto.Id.Value);
                    if (existing != null)
                    {
                        // Update in place
                        existing.Postleitzahl = dto.Postleitzahl;
                        existing.Ort = dto.Ort;
                        existing.Strasse = dto.Strasse;
                        existing.Hausnummer = dto.Hausnummer ?? string.Empty;
                    }
                    else
                    {
                        // ID provided but doesn't exist - add as new with new ID
                        person.Anschriften.Add(new Anschrift(
                            id: Guid.NewGuid(), // NEW ID!
                            personId: person.Id,
                            postleitzahl: dto.Postleitzahl,
                            ort: dto.Ort,
                            strasse: dto.Strasse,
                            hausnummer: dto.Hausnummer ?? string.Empty
                        ));
                    }
                }
                else
                {
                    // No ID - definitely new
                    person.Anschriften.Add(new Anschrift(
                        id: Guid.NewGuid(),
                        personId: person.Id,
                        postleitzahl: dto.Postleitzahl,
                        ort: dto.Ort,
                        strasse: dto.Strasse,
                        hausnummer: dto.Hausnummer ?? string.Empty
                    ));
                }
            }

            // 5) Process Phones
            var incomingPhoneIds = phones
                .Where(p => p.Id.HasValue && p.Id.Value != Guid.Empty)
                .Select(p => p.Id!.Value)
                .ToHashSet();

            // Delete phones not in incoming list
            foreach (var phone in existingPhones.Where(p => !incomingPhoneIds.Contains(p.Id)))
            {
                person.Telefonverbindungen.Remove(phone);
                _personRepo.RemoveTelefonverbindung(phone);
            }

            // Update or Add phones
            foreach (var dto in phones)
            {
                if (dto.Id.HasValue && dto.Id.Value != Guid.Empty)
                {
                    // Try to update existing
                    var existing = person.Telefonverbindungen.FirstOrDefault(p => p.Id == dto.Id.Value);
                    if (existing != null)
                    {
                        // Update in place
                        existing.Telefonnummer = dto.Telefonnummer;
                    }
                    else
                    {
                        // ID provided but doesn't exist - add as new with new ID
                        person.Telefonverbindungen.Add(new Telefonverbindung(
                            id: Guid.NewGuid(), // NEW ID!
                            personId: person.Id,
                            telefonnummer: dto.Telefonnummer
                        ));
                    }
                }
                else
                {
                    // No ID - definitely new
                    person.Telefonverbindungen.Add(new Telefonverbindung(
                        id: Guid.NewGuid(),
                        personId: person.Id,
                        telefonnummer: dto.Telefonnummer
                    ));
                }
            }

            // 6) Save changes
            await _personRepo.UpdateAsync(person, ct);

            _logger.LogInformation("UpdateDetails completed successfully");
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
