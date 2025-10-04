using Backend.PeopleRegistry.Api.Dtos;
using Backend.PeopleRegistry.Application.Services;
using Backend.PeopleRegistry.Domain.Person; // your Person entity namespace
using Microsoft.AspNetCore.Mvc;

namespace Backend.PeopleRegistry.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PersonController : ControllerBase
{
    private readonly IPersonService _service;

    public PersonController(IPersonService service) => _service = service;

    // GET api/person
    [HttpGet]
    public async Task<ActionResult<List<PersonDto>>> GetAll(CancellationToken ct)
    {
        var people = await _service.GetAllAsync(ct);
        return Ok(people.Select(ToDto).ToList());
    }

    // GET api/person/{name}
    [HttpGet("search/{name}")]
    public async Task<ActionResult<List<PersonDto>>> SearchByName([FromRoute] string name, CancellationToken ct)
    {
        var people = await _service.SearchByName(name, ct);
        return Ok(people.Select(ToDto).ToList());
    }

    // GET api/person/id/{id}
    [HttpGet("id/{id:guid}")]
     public async Task<ActionResult<PersonDto>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var person = await _service.GetByIdAsync(id, ct);
        if (person is null) return NotFound();
        return Ok(ToDto(person));
    }

    // POST api/person
    [HttpPost]
    public async Task<ActionResult<PersonDto>> Create([FromBody] PersonWriteDto dto, CancellationToken ct)
    {
        try
        {
            var created = await _service.CreateAsync(dto.Vorname, dto.Nachname, dto.Geburtsdatum, ct);
            var result = ToDto(created);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ax)
        {
            return ValidationProblem(new ValidationProblemDetails
            {
                Title = "Validation error",
                Detail = ax.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (InvalidOperationException iox)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Duplicate",
                Detail = iox.Message,
                Status = StatusCodes.Status409Conflict
            });
        }
    }

    // PUT api/person/id/{id}
    [HttpPut("id/{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PersonWriteDto dto, CancellationToken ct)
    {
        try
        {
            await _service.UpdateAsync(id, dto.Vorname, dto.Nachname, dto.Geburtsdatum, ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ax)
        {
            return ValidationProblem(new ValidationProblemDetails
            {
                Title = "Validation error",
                Detail = ax.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    // --- mapping ---
    private static PersonDto ToDto(Person p) => new()
    {
        Id = p.Id,
        Vorname = p.Vorname,
        Nachname = p.Nachname,
        Geburtsdatum = p.Geburtsdatum
    };
}
