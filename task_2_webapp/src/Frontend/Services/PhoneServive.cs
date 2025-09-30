using System.Net.Http.Json;
using Frontend.Models.Dto;

public class PhoneService
{
    private readonly HttpClient _httpClient;

    public PhoneService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Hole alle Nummer vom Person mit seinem Id
    public async Task<List<PhoneDto>> GetPhonesByPersonIdAsync(Guid personId)
    {
        return await _httpClient.GetFromJsonAsync<List<PhoneDto>>($"api/phone/{personId}");
    }

    // Aktuelisiere Nummer
    public async Task UpdatePhoneAsync(PhoneDto phone)
    {
        await _httpClient.PutAsJsonAsync($"api/phone/{phone.Id}", phone);
    }

    // Füge neue Nummer hinzu
    public async Task AddPhoneAsync(PhoneDto phone)
    {
        await _httpClient.PostAsJsonAsync("api/phone", phone);
    }
}
