using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TechMoveClient.Models;

/*
Author: PROG7311-2026-EMWVL (Lecturer Repository)
URL: https://github.com/PROG7311-2026-EMWVL/MathAPIClient
Date: [n.d]
Date Accessed: 10 May 2026
*/

namespace TechMoveClient.Services.Api
{
    public class ServiceRequestApiService
    {
        private readonly HttpClient _http;

        public ServiceRequestApiService(HttpClient http)
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
            // If token is null or whitespace, remove Authorization header
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

        // GET all service requests from API
        public async Task<List<ServiceRequestDto>> GetAllAsync()
        {
            var response = await _http.GetAsync("api/servicerequests");

            response.EnsureSuccessStatusCode();

            // Read response body as JSON string
            var json = await response.Content.ReadAsStringAsync();

            // Deserialize JSON to list of ServiceRequestDto objects with case-insensitive property matching
            return JsonSerializer.Deserialize<List<ServiceRequestDto>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<ServiceRequestDto>();
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.getasync
        Date: [n.d]
        Date Accessed: 10 May 2026
        */

        // GET service request by ID from API
        public async Task<ServiceRequestDto?> GetByIdAsync(int id)
        {
            var response = await _http.GetAsync($"api/servicerequests/{id}");

            response.EnsureSuccessStatusCode();

            // Read response body as JSON string
            var json = await response.Content.ReadAsStringAsync();

            // Deserialize JSON to ServiceRequestDto object with case-insensitive property matching
            return JsonSerializer.Deserialize<ServiceRequestDto>(json,
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

        // POST new service request to API
        public async Task CreateAsync(ServiceRequestCreateDto request)
        {
            // Serialize ServiceRequestCreateDto object to JSON string
            var json = JsonSerializer.Serialize(request);

            // Create StringContent with JSON payload and appropriate media type
            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _http.PostAsync("api/servicerequests", content);

            // Provide detailed error message if API call fails
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

        // PATCH update service request status
        public async Task UpdateStatusAsync(int id, ServiceRequestStatus status)
        {
            // Serialize ServiceRequestStatus object to JSON string
            var json = JsonSerializer.Serialize(status);

            // Create StringContent with JSON payload and appropriate media type
            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _http.PatchAsync(
                $"api/servicerequests/{id}/status",
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

        // DELETE service request by ID
        public async Task DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync(
                $"api/servicerequests/{id}"
            );

            response.EnsureSuccessStatusCode();
        }
    }
}