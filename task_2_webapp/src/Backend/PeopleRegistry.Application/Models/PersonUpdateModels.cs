namespace Backend.PeopleRegistry.Application.Models;

public sealed record AddressModel(Guid? Id, string Postleitzahl, string Ort, string Strasse, string? Hausnummer);
public sealed record PhoneModel(Guid? Id, string Telefonnummer);
