using Xunit;
using TechMoveAPI.Services;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using TechMoveAPI.Models;

/*
Author: Ivan Stulskij
URL: https://medium.com/@ivanstulskij/unit-testing-in-c-using-xun-585f4ed9fec7
Date: 11 November 2023
Date Accessed: 22 April 2026

Author: Microsoft Learn
URL: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-csharp-with-xunit
Date: [n.d]
Date Accessed: 22 April 2026

Author: Shubhadeep Chattopadhyay
URL: https://medium.com/@shubhadeepchat/unit-testing-in-net-core-6-0-web-api-using-xunit-83a5eb30d79b
Date: 18 July 2022
Date Accessed: 22 April 2026

Author: Jeremy Wells
URL: https://medium.com/swlh/testing-an-asp-net-core-service-with-xunit-f18225d9b22a
Date: 3 August 2020
Date Accessed: 22 April 2026

Author: Yegor Sychev
URL: https://medium.com/@yegor-sychev/mocking-httpclient-in-c-af1f03da28e9
Date: 15 January 2025
Date Accessed: 22 April 2026

Author: Hamid Mosalla
URL: https://hamidmosalla.com/2017/02/08/mock-httpclient-using-httpmessagehandler/
Date: 8 February 2017
Date Accessed: 22 April 2026

Author: Ferry Utomo
URL: https://www.linkedin.com/pulse/c-unit-testing-impossible-ferry-utomo
Date: 16 April 2023
Date Accessed: 22 April 2026

Author: Pramod Choudhari
URL: https://medium.com/@pramod.choudhari/moq-http-response-message-483bf0854f37
Date: 26 December 2024
Date Accessed: 22 April 2026

Author: Chris Sainty
URL: https://chrissainty.com/unit-testing-with-httpclient/
Date: 8 February 2018
Date Accessed: 22 April 2026
*/
namespace ProjectTests.Services
{
    public class ExchangeRateServiceTests
    {
        private ExchangeRateService CreateService(string jsonResponse, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var httpClient = new HttpClient(new FakeHttpRateHandler(jsonResponse, statusCode));

            var options = Options.Create(new ApiSettings
            {
                ExchangeRateApiKey = "test-key"
            });

            return new ExchangeRateService(httpClient, options);
        }

        // Success Test
        [Fact]
        public async Task GetRateAsync_ShouldReturnRate_WhenApiResponseIsValid()
        {
            // Arrange
            var json = """
            {
                "conversion_rates": {
                    "ZAR": 18.50
                }
            }
            """;

            var service = CreateService(json);

            // Act
            var result = await service.GetRateAsync("USD", "ZAR");

            // Assert
            Assert.Equal(18.50m, result);
        }

        // Missing Field
        [Fact]
        public async Task GetRateAsync_ShouldReturn1_WhenConversionRatesMissing()
        {
            // Arrange
            var json = "{}";

            var service = CreateService(json);

            // Act
            var result = await service.GetRateAsync("USD", "ZAR");

            Assert.Equal(1m, result);
        }

        // Wrong Currency
        [Fact]
        public async Task GetRateAsync_ShouldReturn1_WhenCurrencyNotFound()
        {
            // Arrange
            var json = """
            {
                "conversion_rates": {
                    "EUR": 0.9
                }
            }
            """;

            var service = CreateService(json);

            // Act
            var result = await service.GetRateAsync("USD", "ZAR");

            // Assert
            Assert.Equal(1m, result);
        }

        // Http Failure
        [Fact]
        public async Task GetRateAsync_ShouldReturn1_WhenHttpFails()
        {
            // Arrange
            var service = CreateService("", HttpStatusCode.InternalServerError);

            // Act
            var result = await service.GetRateAsync("USD", "ZAR");

            // Assert
            Assert.Equal(1m, result);
        }

        // Invalid JSON Test
        [Fact]
        public async Task GetRateAsync_ShouldReturn1_WhenJsonInvalid()
        {
            // Arrange
            var json = "INVALID_JSON";

            var service = CreateService(json);

            // Act
            var result = await service.GetRateAsync("USD", "ZAR");

            // Assert
            Assert.Equal(1m, result);
        }
    }

    // Fake Http Handler to simulate API responses
    public class FakeHttpRateHandler : HttpMessageHandler
    {
        private readonly string _response;
        private readonly HttpStatusCode _statusCode;

        public FakeHttpRateHandler(string response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            _response = response;
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = _statusCode,
                Content = new StringContent(_response, Encoding.UTF8, "application/json")
            });
        }
    }
}