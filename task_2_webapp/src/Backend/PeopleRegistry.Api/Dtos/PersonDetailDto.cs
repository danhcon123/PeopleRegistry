namespace Backend.PeopleRegistry.Api.Dtos;

public sealed class PersonDetailDto : PersonDto
{
    public List<AddressDto> Anschriften { get; set; } = [];
    public List<PhoneDto> Telefonverbindungen { get; set; } = [];
}