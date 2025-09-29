namespace Frontend.Models.Ui;

public sealed class AddressVm
{
    public Guid Id { get; set; }
    public int PersonId { get; set; }

    public string Strasse { get; set; } = "";
    public string? Hausnummer { get; set; }
    public string Postleitzahl { get; set; } = "";
    public string Ort { get; set; } = "";
}
