using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using Frontend.Models.Dto;

namespace Frontend.Services;

public class PersonService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PersonService> _logger;

    public PersonService(HttpClient httpClient, ILogger<PersonService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }


    /// <summary>
    /// Ruft alle Personen vom Backend ab.
    /// GET /api/person
    /// </summary>
    public async Task<List<PersonDto>> GetAllPersonsAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Lade alle Personen...");

        try
        {
            var data = await _httpClient.GetFromJsonAsync<List<PersonDto>>("api/person", ct);
            _logger.LogInformation("{Count} Personen erfolgreich geladen.", data?.Count ?? 0);
            return data ?? new List<PersonDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Laden aller Personen.");
            throw;
        }
    }

    /// <summary>
    /// Sucht Personen nach Namen.
    /// GET /api/person/search/{name}
    /// </summary>
    public async Task<List<PersonDto>> GetPersonsByNameAsync(string name, CancellationToken ct)
    {
        _logger.LogInformation("Suche Personen mit Namen: {Name}", name);
        try
        {
            var data = await _httpClient.GetFromJsonAsync<List<PersonDto>>($"api/person/search/{name}", ct);
            _logger.LogInformation("{Count} Personen gefunden für Suchbegriff '{Name}'.", data?.Count ?? 0, name);
            return data ?? new List<PersonDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler bei der Personensuche nach '{Name}'.", name);
            throw;
        }
    }


    // ------------------------------------
    // --- Detailansicht (mit Kindern) ---
    // ------------------------------------

    // --- Create ---

    
    /// <summary>
    /// Erstellt eine neue Person.
    /// POST /api/person
    /// </summary>
    public async Task<PersonDto?> AddPersonAsync(PersonDto person, CancellationToken ct = default){
        _logger.LogInformation("Erstelle neue Person: {Vorname} {Nachname}", person.Vorname, person.Nachname);
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/person", person, ct);
            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<PersonDto>(cancellationToken: ct);
            _logger.LogInformation("Neue Person erfolgreich erstellt (ID: {Id})", created?.Id);
            return created;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Erstellen einer neuen Person.");
            throw;
        }
    }

    /// <summary>
    /// Ruft die Detailansicht einer Person ab (inkl. Adressen & Telefonnummern).
    /// GET /api/person/details/{id}
    /// </summary>
    public async Task<PersonDto?> GetPersonDetailsAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogInformation("Lade Detaildaten für Person ID: {Id}", id);

        try
        {
            return await _httpClient.GetFromJsonAsync<PersonDto>($"api/person/details/{id}", ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Laden der Detaildaten für ID {Id}", id);
            throw;
        }
    }

    /// <summary>
    /// Aktualisiert die Detaildaten einer Person (inkl.Adressen & Telefonnummern.)
    /// PUT /api/person/details/{id}
    /// </summary>
    public async Task<PersonDto?> UpdatePersonDetailsAsync(Guid id, UpdatePersonDetailsRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Aktualisiere Detaildaten für PersonId: {Id}", id);
        _logger.LogInformation("Outgoing counts: addresses={Addr}, phones={Phones}",
            request.Anschriften?.Count ?? 0, request.Telefonverbindungen?.Count ?? 0);
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/person/details/{id}", request, ct);
            response.EnsureSuccessStatusCode(); // Löst Ausnahme aus, falls kein 2xx-Status zurückkommt

            var updated = await response.Content.ReadFromJsonAsync<PersonDto>(ct);
            _logger.LogInformation("Detaildaten für Person ID {Id} erfolgreich aktualisiert.", id);
            return updated;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Aktualisieren der Detaildaten für ID {Id}", id);
            throw;
        }
    }
}