using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.Dto;

public sealed class AddressDto
{
    public Guid? Id { get; init; }
    public Guid PersonId { get; init; }

    [Required, StringLength(200)]
    public string Strasse { get; set; } = string.Empty;

    [StringLength(10)]
    public string? Hausnummer { get; set; }

    [Required, StringLength(10)]
    public string Postleitzahl { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Ort { get; set; } = string.Empty;
}