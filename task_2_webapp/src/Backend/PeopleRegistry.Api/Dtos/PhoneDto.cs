namespace Backend.PeopleRegistry.Api.Dtos;

public sealed class PhoneDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string Telefonnummer { get; set; } = "";
}