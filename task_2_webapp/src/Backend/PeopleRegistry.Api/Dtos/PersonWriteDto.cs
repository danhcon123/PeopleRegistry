using System.ComponentModel.DataAnnotations;

namespace Backend.PeopleRegistry.Api.Dtos;

public sealed class PersonWriteDto
{
    [Required, StringLength(100)]
    public string Vorname { get; set; } = "";

    [Required, StringLength(100)]
    public string Nachname { get; set; } = "";

    public DateTime? Geburtsdatum { get; set; }
}
