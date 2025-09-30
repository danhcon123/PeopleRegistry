using Frontend.Models.Dto;

namespace Frontend.Services;
public class PersonService
{
    private readonly HttpClient _httpClient;

    public PersonService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Alle Personen holen
    public async Task<List<PersonDto>> GetAllPersonsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<PersonDto>>("api/person");
    }

    // Fetch person by name (for search functionality)
    public async Task<List<PersonDto>> GetPersonsByNameAsync(string name)
    {
        return await _httpClient.GetFromJsonAsync<List<PersonDto>>($"api/person/{name}");
    }

    // Update person information
    public async Task UpdatePersonAsync(PersonDto person)
    {
        await _httpClient.PutAsJsonAsync($"api/person/{person.Id}", person);
    }

    // Add a new person
    public async Task AddPersonAsync(PersonDto person)
    {
        await _httpClient.PostAsJsonAsync("api/person", person);
    }
}