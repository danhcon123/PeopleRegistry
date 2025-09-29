namespace Frontend.Models.Ui;

public sealed class PhoneVm
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string Telefonnummer { get; set; } = "";
}
