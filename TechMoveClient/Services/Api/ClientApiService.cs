using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TechMoveClient.Models;

namespace TechMoveClient.Services.Api
{
    public class ClientApiService
    {
        private readonly HttpClient _http;

        public ClientApiService(HttpClient http)
        {
            _http = http;
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.headers.authenticationheadervalue
        Date: [n.d]
        Date Accessed: 10 May 2026
        */

        // Attach or remove JWT token for authenticated API requests
        public void SetToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization = null;
                return;
            }

            // Set Bearer token for API authentication
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.getasync
        Date: [n.d]
        Date Accessed: 10 May 2026
        */

        // GET all clients from API
        public async Task<List<Client>> GetAllAsync()
        {
            var response = await _http.GetAsync("api/clients");

            response.EnsureSuccessStatusCode();

            // Read response body as JSON string
            var json = await response.Content.ReadAsStringAsync();

            // Deserialize JSON to list of Client objects with case-insensitive property matching
            return JsonSerializer.Deserialize<List<Client>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<Client>();
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.getasync
        Date: [n.d]
        Date Accessed: 10 May 2026
        */

        // GET client by ID from API
        public async Task<Client?> GetByIdAsync(int id)
        {
            var response = await _http.GetAsync($"api/clients/{id}");

            response.EnsureSuccessStatusCode();

            // Read response body as JSON string
            var json = await response.Content.ReadAsStringAsync();

            // Deserialize JSON to Client object with case-insensitive property matching
            return JsonSerializer.Deserialize<Client>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.postasync
        Date: [n.d]
        Date Accessed: 10 May 2026
        */

        // POST new client to API
        public async Task CreateAsync(Client client)
        {
            // Serialize Client object to JSON string
            var json = JsonSerializer.Serialize(client);

            // Create StringContent with JSON payload and appropriate media type
            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _http.PostAsync("api/clients", content);

            response.EnsureSuccessStatusCode();
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.putasync
        Date: [n.d]
        Date Accessed: 10 May 2026
        */

        // PUT update existing client
        public async Task UpdateAsync(int id, Client client)
        {
            // Serialize Client object to JSON string
            var json = JsonSerializer.Serialize(client);

            // Create StringContent with JSON payload and appropriate media type
            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _http.PutAsync($"api/clients/{id}", content);

            response.EnsureSuccessStatusCode();
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.deleteasync
        Date: [n.d]
        Date Accessed: 10 May 2026
        */

        // DELETE client by ID
        public async Task DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/clients/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}