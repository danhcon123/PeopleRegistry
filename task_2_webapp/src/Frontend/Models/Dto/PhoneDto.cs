using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.Dto;

public sealed class PhoneDto
{
    public Guid? Id { get; set; }
    public Guid PersonId { get; set; }
    public string Telefonnummer { get; set; } = string.Empty;
}