namespace Backend.PeopleRegistry.Api.Dtos;

public sealed class AddressDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string Postleitzahl { get; set; } = "";
    public string Ort { get; set; } = "";
    public string Strasse { get; set; } = "";
    public string Hausnummer { get; set; } = "";
}