using Xunit;
using TechMoveAPI.Services;
using TechMoveAPI.Models;
using TechMoveAPI.Data;
using TechMoveAPI.Domain.Strategy;
using TechMoveAPI.Domain.Observers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using Moq;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Net;
using System.Text;

/*
Author: Ivan Stulskij
URL: https://medium.com/@ivanstulskij/unit-testing-in-c-using-xun-585f4ed9fec7
Date: 11 November 2023
Date Accessed: 21 April 2026

Author: Microsoft Learn
URL: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-csharp-with-xunit
Date: [n.d]
Date Accessed: 21 April 2026

Author: Shubhadeep Chattopadhyay
URL: https://medium.com/@shubhadeepchat/unit-testing-in-net-core-6-0-web-api-using-xunit-83a5eb30d79b
Date: 18 July 2022
Date Accessed: 21 April 2026

Author: Gustavo Lage
URL: https://medium.com/@gustavolage/using-xunit-net-moq-and-inmemorydatabase-for-more-efficient-unit-testing-9763949dec2f
Date: 12 May 2020
Date Accessed: 21 April 2026

Author: Jeremy Wells
URL: https://medium.com/swlh/testing-an-asp-net-core-service-with-xunit-f18225d9b22a
Date: 3 August 2020
Date Accessed: 21 April 2026

Author: Arpit Shrivastava
URL: https://www.c-sharpcorner.com/article/in-memory-databases-unit-testing-with-c-sharp-efcore-and-xunit/
Date: [n.d]
Date Accessed: 21 April 2026
*/

namespace ProjectTests.Services
{
    public class ServiceRequestServiceTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            return new AppDbContext(options);
        }

        // Helper method: fixed exhange rate service
        private ExchangeRateService GetExchangeRateService()
        {
            var httpClient = new HttpClient(new FakeHttpHandler());

            var options = Options.Create(new ApiSettings
            {
                ExchangeRateApiKey = "test-key"
            });

            return new ExchangeRateService(httpClient, options);
        }

        private StrategyFactory GetFactory()
        {
            var exchangeService = GetExchangeRateService();
            return new StrategyFactory(exchangeService);
        }

        // Testing Data
        private ServiceRequest CreateRequest(ServiceRequestStatus status = ServiceRequestStatus.Pending)
        {
            return new ServiceRequest
            {
                ServiceRequestId = 1,
                Description = "Test Request",
                Cost = 100,
                Currency = "EUR",
                Status = status,
                ContractId = 1
            };
        }

        // Can Delete
        [Fact]
        public void CanDelete_ShouldReturnTrue_WhenPending()
        {
            // Arrange
            var service = new ServiceRequestService(GetInMemoryContext(), GetFactory());

            // Act
            var request = CreateRequest(ServiceRequestStatus.Pending);

            // Assert
            Assert.True(service.CanDelete(request));
        }

        [Fact]
        public void CanDelete_ShouldReturnFalse_WhenNotPending()
        {
            // Arrange
            var service = new ServiceRequestService(GetInMemoryContext(), GetFactory());

            // Act
            var request = CreateRequest(ServiceRequestStatus.Completed);

            // Assert
            Assert.False(service.CanDelete(request));
        }

        // Observer and Status Update
        [Fact]
        public void UpdateStatus_ShouldChangeStatus()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ServiceRequestService(context, GetFactory());

            var request = CreateRequest();

            context.ServiceRequests.Add(request);
            context.SaveChanges();

            // Act
            service.UpdateStatus(request, ServiceRequestStatus.Completed);

            // Assert
            Assert.Equal(ServiceRequestStatus.Completed, request.Status);
        }

        // DB
        [Fact]
        public void UpdateStatus_ShouldPersistToDatabase()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ServiceRequestService(context, GetFactory());

            var request = CreateRequest();

            context.ServiceRequests.Add(request);
            context.SaveChanges();

            // Act
            service.UpdateStatus(request, ServiceRequestStatus.Completed);

            var updated = context.ServiceRequests.First();

            // Assert
            Assert.Equal(ServiceRequestStatus.Completed, updated.Status);
        }

        // Async Strategy
        [Fact]
        public async Task CalculateCostAsync_ShouldReturnValidResult()
        {
            // Arrange
            var service = new ServiceRequestService(GetInMemoryContext(), GetFactory());

            // Act
            var result = await service.CalculateCostAsync("ZAR", 100);

            // Assert
            Assert.True(result >= 0);
        }

        [Fact]
        public void Observer_ShouldTriggerBothObservers()
        {
            // Arrange
            var subject = new ServiceRequestSubject
            {
                RequestId = 1
            };

            subject.Attach(new NotificationService());
            subject.Attach(new DriverApp());

            // Act
            subject.UpdateStatus("Completed");

            // Assert
            Assert.Contains(NotificationService.Notifications, m => m.Contains("Completed"));
            Assert.Contains(DriverApp.DriverMessages, m => m.Contains("Completed"));
        }
    }

    // Fake Http Handler to simulate API responses for ExchangeRateService tests
    public class FakeHttpHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var json = """
            {
                "conversion_rates": {
                    "ZAR": 18.50
                }
            }
            """;

            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        }
    }
}