using System.Net.Http.Json;
using Frontend.Models.Dto;

public class AddressService
{
    private readonly HttpClient _httpClient;

    public AddressService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Get all addresses for a specific person
    public async Task<List<AddressDto>> GetAddressesByPersonIdAsync(Guid personId)
    {
        return await _httpClient.GetFromJsonAsync<List<AddressDto>>($"api/address/{personId}");
    }

    // Update an address
    public async Task UpdateAddressAsync(AddressDto address)
    {
        await _httpClient.PutAsJsonAsync($"api/address/{address.Id}", address);
    }

    // Add a new address
    public async Task AddAddressAsync(AddressDto address)
    {
        await _httpClient.PostAsJsonAsync("api/address", address);
    }
}
