using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TechMoveAPI.Services;
using TechMoveAPI.Models;
using TechMoveAPI.Data;

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
    public class ClientServiceTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            return new AppDbContext(options);
        }

        // helper method
        private Client CreateValidClient(int id = 1)
        {
            return new Client
            {
                ClientId = id,
                Name = "Test Client",
                ContactDetails = "0812345678",
                Region = "Gauteng"
            };
        }

        private ContractEntity CreateContract(int clientId = 1)
        {
            return new ContractEntity
            {
                ContractId = 1,
                ClientId = clientId,
                ServiceLevel = "Standard",
                Status = ContractStatus.Active,
                StartDate = System.DateTime.Today,
                EndDate = System.DateTime.Today.AddDays(10)
            };
        }

        [Fact]
        public void CanDelete_ShouldReturnTrue_WhenNoContractsExist()
        {
            // Arrange
            var context = GetInMemoryContext();

            var client = CreateValidClient();

            context.Clients.Add(client);
            context.SaveChanges();

            var service = new ClientService(context);

            // Act
            var result = service.CanDelete(client);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanDelete_ShouldReturnFalse_WhenContractsExist()
        {
            // Arrange
            var context = GetInMemoryContext();

            var client = CreateValidClient();
            var contract = CreateContract(client.ClientId);

            context.Clients.Add(client);
            context.Contracts.Add(contract);
            context.SaveChanges();

            var service = new ClientService(context);

            // Act
            var result = service.CanDelete(client);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DeleteClient_ShouldRemoveClient_FromDatabase()
        {
            // Arrange
            var context = GetInMemoryContext();

            var client = CreateValidClient();

            context.Clients.Add(client);
            context.SaveChanges();

            var service = new ClientService(context);

            // Act
            service.DeleteClient(client);

            // Assert
            var exists = context.Clients.Any(c => c.ClientId == client.ClientId);
            Assert.False(exists);
        }

        [Fact]
        public void DeleteClient_ShouldThrow_WhenClientIsNull()
        {
            var context = GetInMemoryContext();
            var service = new ClientService(context);

            // Act & Assert
            Assert.Throws<System.ArgumentNullException>(() =>
            {
                service.DeleteClient(null);
            });
        }

        [Fact]
        public void CanDelete_ShouldReturnFalse_WhenMultipleContractsExist()
        {
            // Arrange
            var context = GetInMemoryContext();

            var client = CreateValidClient();
            var contract1 = CreateContract(client.ClientId);
            var contract2 = CreateContract(client.ClientId);

            contract2.ContractId = 2;

            context.Clients.Add(client);
            context.Contracts.AddRange(contract1, contract2);
            context.SaveChanges();

            var service = new ClientService(context);

            // Act
            var result = service.CanDelete(client);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void CanDelete_ShouldReturnTrue_AfterContractsAreRemoved()
        {
            // Arrange
            var context = GetInMemoryContext();

            var client = CreateValidClient();
            var contract = CreateContract(client.ClientId);

            context.Clients.Add(client);
            context.Contracts.Add(contract);
            context.SaveChanges();

            var service = new ClientService(context);

            // Act
            Assert.False(service.CanDelete(client));

            context.Contracts.Remove(contract);
            context.SaveChanges();

            // Assert
            Assert.True(service.CanDelete(client));
        }

        [Fact]
        public void DeleteClient_ShouldOnlyRemoveSpecifiedClient()
        {
            // Arrange
            var context = GetInMemoryContext();

            var client1 = CreateValidClient(1);
            var client2 = CreateValidClient(2);
            client2.Name = "Second Client";
            client2.ContactDetails = "0822222222";

            context.Clients.AddRange(client1, client2);
            context.SaveChanges();

            var service = new ClientService(context);

            // Act
            service.DeleteClient(client1);

            var remaining = context.Clients.ToList();

            // Assert
            Assert.Single(remaining);
            Assert.Equal(2, remaining.First().ClientId);
        }
    }
}