namespace Backend.PeopleRegistry.Domain.Telefonverbindung;

public class Telefonverbindung
{
    public Telefonverbindung(Guid id, Guid personId, String telefonnummer)
    {
        Id = id;
        PersonId = personId;
        Telefonnummer = telefonnummer;
    }

    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public String Telefonnummer { get; set; }

    // Navigier zurück zum Person
    public Backend.PeopleRegistry.Domain.Person.Person? Person { get; private set; }

}