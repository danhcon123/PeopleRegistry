namespace Frontend.Models.Dto;

public sealed class UpdatePersonDetailsRequest
{
    public string Vorname { get; set; } = string.Empty;
    public string Nachname { get; set; } = string.Empty;
    public DateTime? Geburtsdatum { get; set; }
    public List<AddressDto> Anschriften { get; set; } = new();
    public List<PhoneDto> Telefonverbindungen { get; set; } = new();
}