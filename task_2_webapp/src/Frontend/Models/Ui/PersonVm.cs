namespace Frontend.Models.Ui;

public sealed class PersonVm
{
    public int Id { get; set; }
    public string IdShort => Id.ToString(); // simple display; keep it int
    public string Vorname { get; set; } = "";
    public string Nachname { get; set; } = "";
    public DateTime? Geburtsdatum { get; set; }

    public List<AddressVm> Addresses { get; set; } = new();
    public List<PhoneVm> Phones { get; set; } = new();
}
