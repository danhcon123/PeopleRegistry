using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.Dto;

public sealed class PersonDto
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Vorname { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Nachname { get; set; } = string.Empty;

    [Required, DataType(DataType.Date)]
    public DateTime? Geburtsdatum { get; set; }

    public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
    public List<PhoneDto> Phones { get; set; } = new List<PhoneDto>();
}