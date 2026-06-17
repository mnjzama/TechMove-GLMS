using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TechMoveClient.Models;

namespace TechMoveClient.Services.Api
{
    public class ContractApiService
    {
        private readonly HttpClient _http;

        public ContractApiService(HttpClient http)
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

        // GET all contracts (supports optional filtering via query parameters)
        public async Task<List<ContractDto>> GetAllAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            ContractStatus? status = null)
        {
            // Build query string dynamically based on provided filters
            var queryParams = new List<string>();

            if (startDate.HasValue)
                queryParams.Add($"startDate={startDate.Value:yyyy-MM-dd}");

            if (endDate.HasValue)
                queryParams.Add($"endDate={endDate.Value:yyyy-MM-dd}");

            if (status.HasValue)
                queryParams.Add($"status={status.Value}");

            var url = "api/contracts";

            // Append query parameters to URL if any filters are applied
            if (queryParams.Any())
                url += "?" + string.Join("&", queryParams);

            var response = await _http.GetAsync(url);

            response.EnsureSuccessStatusCode();

            // Read response body as JSON string
            var json = await response.Content.ReadAsStringAsync();

            // Deserialize JSON to list of ContractDto objects with case-insensitive property matching
            return JsonSerializer.Deserialize<List<ContractDto>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<ContractDto>();
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.getasync
        Date: [n.d]
        Date Accessed: 10 May 2026
        */

        // GET contract by ID
        public async Task<ContractDto?> GetByIdAsync(int id)
        {
            var response = await _http.GetAsync($"api/contracts/{id}");

            response.EnsureSuccessStatusCode();

            // Read response body as JSON string
            var json = await response.Content.ReadAsStringAsync();

            // Deserialize JSON to ContractDto object with case-insensitive property matching
            return JsonSerializer.Deserialize<ContractDto>(json,
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

        // POST create new contract
        public async Task CreateAsync(ContractCreateDto contract)
        {
            // Serialize ContractCreateDto object to JSON string
            var json = JsonSerializer.Serialize(contract);

            // Create StringContent with JSON payload and appropriate media type
            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _http.PostAsync("api/contracts", content);

            // Provide detailed error feedback if API request fails
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {error}");
            }
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.patchasync
        Date: [n.d]
        Date Accessed: 10 May 2026
        */

        // PATCH contract status
        public async Task UpdateStatusAsync(int id, ContractStatus status)
        {
            // Serialize status enum to JSON string
            var json = JsonSerializer.Serialize(status);

            // Create StringContent with JSON payload and appropriate media type
            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _http.PatchAsync(
                $"api/contracts/{id}/status",
                content
            );

            response.EnsureSuccessStatusCode();
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.deleteasync
        Date: [n.d]
        Date Accessed: 10 May 2026
        */

        // DELETE contract by ID
        public async Task DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/contracts/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}