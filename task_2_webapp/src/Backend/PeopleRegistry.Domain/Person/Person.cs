using Backend.PeopleRegistry.Domain.Anschrift;
using Backend.PeopleRegistry.Domain.Telefonverbindung;

namespace Backend.PeopleRegistry.Domain.Person;

public class Person
{
    private Person() { }

    public Person(Guid id, string vorname, string nachname, DateTime geburtsdatum)
    {
        Id = id;
        Vorname = vorname;
        Nachname = nachname;
        Geburtsdatum = geburtsdatum;
        Anschriften = new List<Backend.PeopleRegistry.Domain.Anschrift.Anschrift>();
        Telefonverbindungen = new List<Backend.PeopleRegistry.Domain.Telefonverbindung.Telefonverbindung>();
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Vorname { get; set; } = string.Empty;
    public string Nachname { get; set; } = string.Empty;
    public DateTime? Geburtsdatum { get; set; }
    public ICollection<Backend.PeopleRegistry.Domain.Anschrift.Anschrift> Anschriften { get; private set; } = new List<Backend.PeopleRegistry.Domain.Anschrift.Anschrift>();
    public ICollection<Backend.PeopleRegistry.Domain.Telefonverbindung.Telefonverbindung> Telefonverbindungen { get; private set; } = new List<Backend.PeopleRegistry.Domain.Telefonverbindung.Telefonverbindung>();
}