namespace Backend.PeopleRegistry.Domain.Anschrift;

public class Anschrift
{
    public Anschrift(Guid id, Guid personId,
                     String hausnummer, String strasse,
                     String postleitzahl, String ort)
    {
        Id = id;
        PersonId = personId;
        Hausnummer = hausnummer;
        Strasse = strasse;
        Postleitzahl = postleitzahl;
        Ort = ort;
    }

    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public String Hausnummer { get; set; }
    public String Strasse { get; set; }
    public String Postleitzahl { get; set; }
    public String Ort { get; set; }

    // Navigier zurück zum Person
    public Backend.PeopleRegistry.Domain.Person.Person? Person { get; private set; }
    
}