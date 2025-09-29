using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.Dto;

public sealed class PhoneDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }

    [Required, StringLength(20)]
    [RegularExpression(@"^\+?[1-9]\d{1,14}$",
    ErrorMessage = "Bitte im internationalen Format (E.164), z. B. +4915123456789.")]
    public string Telefonnummer { get; set; } = string.Empty;
}