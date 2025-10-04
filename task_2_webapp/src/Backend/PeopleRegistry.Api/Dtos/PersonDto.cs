namespace Backend.PeopleRegistry.Api.Dtos;

public class PersonDto
{
    public Guid Id { get; set; }
    public string Vorname { get; set; } = "";
    public string Nachname { get; set; } = "";
    public DateTime? Geburtsdatum { get; set; }
}