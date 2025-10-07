using Backend.PeopleRegistry.Api.Dtos;
using Backend.PeopleRegistry.Application.Services;
using Backend.PeopleRegistry.Application.Models;
using Backend.PeopleRegistry.Domain.Person; // your Person entity namespace
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.PeopleRegistry.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PersonController : ControllerBase
{
    private readonly IPersonService _service;
    private readonly ILogger<PersonController> _logger;
    public PersonController(IPersonService service, ILogger<PersonController> logger)
    {
        _service = service;
        _logger = logger;
    }


    // ===========================
    //           READ
    // ===========================


    /// <summary>
    /// Liefert alle Personen (Stammdaten).
    /// GET api/person
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<PersonDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PersonDto>>> GetAll(CancellationToken ct)
    {
        _logger.LogInformation("GET /api/person → GetAll gestartet");
        var people = await _service.GetAllAsync(ct);
        var result = people.Select(ToDto).ToList();
        _logger.LogInformation("GET /api/person → {Count} Personen gefunden", result.Count);
        return Ok(result);
    }


    /// <summary>
    /// Sucht Personen anhand eines Namens oder Namensausschnitts.
    /// GET api/person/{name}
    /// </summary>
    [HttpGet("search/{name}")]
    [ProducesResponseType(typeof(List<PersonDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PersonDto>>> SearchByName([FromRoute] string name, CancellationToken ct)
    {
        _logger.LogInformation("GET /api/person/search/{{name}} → Suche gestartet (Query='{Query}')", name);
        var people = await _service.SearchByName(name, ct);
        var result = people.Select(ToDto).ToList();
        _logger.LogInformation("GET /api/person/search/{name}", result.Count);
        return Ok(result);
    }

    /// <summary>
    /// Liefert eine Person per ID (Stammdaten).
    /// GET api/id/{id}
    /// </summary>
    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonDto>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        _logger.LogInformation("GET /api/person/id/{{id}} → Abruf gestartet (Id={PersonId})", id);
        var person = await _service.GetByIdAsync(id, ct);
        if (person is null)
        {
            return NotFound();
        }
        return Ok(ToDto(person));
    }


    // ===========================
    //          CREATE
    // ===========================

    /// <summary>
    /// Legt eine neue Person (Stammdaten) an.
    /// POST api/person
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PersonDto>> Create([FromBody] PersonWriteDto dto, CancellationToken ct)
    {
        if (dto is null)
        {
            _logger.LogWarning("POST /api/person → Request-Body fehlt");
            return BadRequest();
        }

        try
        {
            var created = await _service.CreateAsync(dto.Vorname, dto.Nachname, dto.Geburtsdatum, ct);
            var result = ToDto(created);
            _logger.LogInformation("POST /api/person → erstellt (Id={PersonId})", result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ax)
        {
            _logger.LogWarning(ax, "POST /api/person → Validierungsfehler: {Message}", ax.Message);
            return ValidationProblem(new ValidationProblemDetails
            {
                Title = "Validation error",
                Detail = ax.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (InvalidOperationException iox)
        {
            _logger.LogWarning(iox, "POST /api/person → Duplikat erkannt: {Message}", iox.Message);
            return Conflict(new ProblemDetails
            {
                Title = "Duplicate",
                Detail = iox.Message,
                Status = StatusCodes.Status409Conflict
            });
        }
    }

    // ===========================
    //          DETAILS
    // ===========================


    /// <summary>
    /// Liefert eine Person inkl. zugehöriger Daten (Anschriften, Telefonverbindungen).
    /// GET details/{id}
    /// </summary>
    [HttpGet("details/{id:guid}")]
    [ProducesResponseType(typeof(PersonDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonDetailDto>> GetDetails(Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Fetching details for person {PersonId}", id);

        var person = await _service.GetDetailsAsync(id, ct);
        if (person is null)
        {
            _logger.LogWarning("Person {PersonId} not found", id);
            return NotFound();
        }

        var dto = ToDetailDto(person);
        return Ok(dto);
    }

    /// <summary>
    /// Aktualisiert Stammdaten, Anschriften und Telefonverbindungen einer Person und gibt die aktualisierten Details zurück.
    /// PUT details/{id}
    /// </summary>
    [HttpPut("details/{id:guid}")]
    public async Task<ActionResult<PersonDetailDto>> UpdateDetails(Guid id, [FromBody] UpdatePersonDetailsRequest dto, CancellationToken ct)
    {
        var addressModels = (dto.Anschriften ?? []).Select(a =>
            new AddressModel(a.Id, a.Postleitzahl, a.Ort, a.Strasse, a.Hausnummer));
        var phoneModels = (dto.Telefonverbindungen ?? []).Select(p =>
            new PhoneModel(p.Id, p.Telefonnummer));

    var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    });

    _logger.LogInformation("Received UpdateDetails for Person {PersonId}. Payload: {Payload}", id, json);
        await _service.UpdateDetailsAsync(
        id,
        dto.Vorname,
        dto.Nachname,
        dto.Geburtsdatum,
        addressModels,
        phoneModels,
        ct);

        var updated = await _service.GetDetailsAsync(id, ct);
        if (updated is null) return NotFound();

        return Ok(ToDetailDto(updated));
    }

    // ===========================
    //          Mapping
    // ===========================

    /// <summary>
    /// Mappt eine <see cref="Person"/> (Stammdaten) auf <see cref="PersonDto"/>.
    /// </summary>
    private static PersonDto ToDto(Person p) => new()
    {
        Id = p.Id,
        Vorname = p.Vorname,
        Nachname = p.Nachname,
        Geburtsdatum = p.Geburtsdatum
    };

    /// <summary>
    /// Wandelt eine Person inklusive abhängiger Daten in ein PersonDetailDto um.
    /// </summary>
    private static PersonDetailDto ToDetailDto(Person p) => new()
    {
        Id = p.Id,
        Vorname = p.Vorname,
        Nachname = p.Nachname,
        Geburtsdatum = p.Geburtsdatum,
        Anschriften = p.Anschriften.Select(a => new AddressDto
        {
            Id = a.Id,
            PersonId = a.PersonId,
            Postleitzahl = a.Postleitzahl,
            Ort = a.Ort,
            Strasse = a.Strasse,
            Hausnummer = a.Hausnummer
        }).ToList(),
        Telefonverbindungen = p.Telefonverbindungen.Select(t => new PhoneDto
        {
            Id = t.Id,
            PersonId = t.PersonId,
            Telefonnummer = t.Telefonnummer
        }).ToList()
    };
}
