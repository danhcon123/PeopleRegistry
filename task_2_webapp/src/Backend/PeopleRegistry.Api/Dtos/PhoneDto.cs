using System.ComponentModel.DataAnnotations;

namespace Backend.PeopleRegistry.Api.Dtos;

public sealed class PhoneDto
{
    public Guid? Id { get; set; }
    public Guid? PersonId { get; set; }

    [Required, StringLength(20)]
    public string Telefonnummer { get; set; } = "";
}