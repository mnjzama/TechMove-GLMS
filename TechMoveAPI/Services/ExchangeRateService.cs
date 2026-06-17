using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using TechMoveAPI.Models;

namespace TechMoveAPI.Services
{
    public class ExchangeRateService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient
        Date: [n.d]
        Date Accessed: 16 April 2026
        */
        public ExchangeRateService(HttpClient client, IOptions<ApiSettings> config)
        {
            _client = client;
            _apiKey = config.Value.ExchangeRateApiKey?.Trim() ?? ""; // Ensure API key is not null and trimmed
        }

        /*
        Author: ExchangeRate-API Documentation
        URL: https://www.exchangerate-api.com/docs/overview
        Date: [n.d]
        Date Accessed: 16 April 2026

        https://medium.com/@jepozdemir/how-to-get-live-currency-exchange-rates-in-c-net-c8026db1f588
        Author: Jiyan Epözdemir
        URL: https://medium.com/@jepozdemir/how-to-get-live-currency-exchange-rates-in-c-net-c8026db1f588
        Date: 9 April 2024
        Date Accessed: 6 June 2026
        */
        public async Task<decimal> GetRateAsync(string fromCurrency, string toCurrency)
        {
            try
            {
                // Validate API key before making request
                if (string.IsNullOrWhiteSpace(_apiKey))
                    throw new Exception("ExchangeRate API key is missing in configuration.");

                // Build the API request URL using base currency
                var url = $"https://v6.exchangerate-api.com/v6/{_apiKey}/latest/{fromCurrency}";

                // Send HTTP request to external exchange rate API
                var response = await _client.GetAsync(url);

                // If request fails, log response body and return fallback rate
                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error fetching exchange rate: {errorBody}");
                    return 1.0m;
                }

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                // Extract conversion rate for target currency
                if (doc.RootElement.TryGetProperty("conversion_rates", out var rates) &&
                    rates.TryGetProperty(toCurrency, out var rateElement))
                {
                    return rateElement.GetDecimal();
                }

                return 1.0m; // Return fallback rate if target currency not found
            }
            catch (Exception ex)
            {
                return 1.0m;
            }
        }
    }
}