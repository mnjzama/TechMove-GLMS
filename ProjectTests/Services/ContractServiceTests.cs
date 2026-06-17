using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
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
    public class ContractServiceTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            return new AppDbContext(options);
        }

        private ContractEntity CreateContract(int id = 1, ContractStatus status = ContractStatus.Active)
        {
            return new ContractEntity
            {
                ContractId = id,
                ClientId = 1,
                ServiceLevel = "Standard",
                Status = status,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(10)
            };
        }

        // Helper method
        private ContractService GetService(AppDbContext context)
        {
            var fileService = new FileService();
            return new ContractService(context, fileService);
        }

        // Business Rules
        [Fact]
        public void CanCreateServiceRequest_ShouldReturnTrue_WhenContractIsActive()
        {
            // Arrange
            var service = GetService(GetInMemoryContext());
            var contract = CreateContract(status: ContractStatus.Active);

            // Act
            var result = service.CanCreateServiceRequest(contract);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanCreateServiceRequest_ShouldReturnFalse_WhenContractNotActive()
        {
            // Arrange
            var service = GetService(GetInMemoryContext());
            var contract = CreateContract(status: ContractStatus.OnHold);

            // Act
            var result = service.CanCreateServiceRequest(contract);

            // Assert
            Assert.False(result);
        }


        // Delete Rules
        [Fact]
        public void CanDelete_ShouldReturnTrue_WhenStatusIsDraft()
        {
            // Arrange
            var service = GetService(GetInMemoryContext());
            var contract = CreateContract(status: ContractStatus.Draft);

            // Act
            var result = service.CanDelete(contract);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanDelete_ShouldReturnFalse_WhenStatusNotDraft()
        {
            // Arrange
            var service = GetService(GetInMemoryContext());
            var contract = CreateContract(status: ContractStatus.Active);

            // Act
            var result = service.CanDelete(contract);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DeleteContract_ShouldRemoveContract_FromDatabase()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = GetService(context);

            var contract = CreateContract();

            context.Contracts.Add(contract);
            context.SaveChanges();

            // Act
            service.DeleteContract(contract);

            var exists = context.Contracts.Any(c => c.ContractId == contract.ContractId);

            // Assert
            Assert.False(exists);
        }

        // Factory Test
        [Fact]
        public void CreateContract_ShouldReturnContract_WithValidServiceLevel()
        {
            // Arrange
            var service = new ContractService(GetInMemoryContext(), new FileService());

            // Act
            var result = service.CreateContract("Express");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Express", result.ServiceLevel);
        }

        // Validation Tests
        [Fact]
        public void IsValidContractDates_ShouldReturnFalse_WhenEndBeforeStart()
        {
            // Arrange
            var service = new ContractService(GetInMemoryContext(), new FileService());

            // Act
            var result = service.IsValidContractDates(DateTime.Today, DateTime.Today.AddDays(-1));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidContractDates_ShouldReturnTrue_WhenEndAfterStart()
        {
            // Arrange
            var service = new ContractService(GetInMemoryContext(), new FileService());

            // Act
            var result = service.IsValidContractDates(DateTime.Today, DateTime.Today.AddDays(5));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasValidDuration_ShouldReturnTrue_WhenAtLeastOneDay()
        {
            // Arrange
            var service = new ContractService(GetInMemoryContext(), new FileService());

            // Act
            var result = service.HasValidDuration(DateTime.Today, DateTime.Today.AddDays(1));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasValidDuration_ShouldReturnFalse_WhenSameDay()
        {
            // Arrange
            var service = new ContractService(GetInMemoryContext(), new FileService());

            // Act
            var result = service.HasValidDuration(DateTime.Today, DateTime.Today);

            // Assert
            Assert.False(result);
        }

        // Update Logic Test
        [Fact]
        public void UpdateStatus_ShouldChangeContractStatus()
        {
            // Arrange
            var service = new ContractService(GetInMemoryContext(), new FileService());

            var contract = CreateContract(status: ContractStatus.Draft);

            // Act
            service.UpdateStatus(contract, ContractStatus.Active);

            // Assert
            Assert.Equal(ContractStatus.Active, contract.Status);
        }

        // Filter Test
        [Fact]
        public void FilterContracts_ShouldFilterByStatus()
        {
            // Arrange
            var context = GetInMemoryContext();

            context.Contracts.Add(CreateContract(1, ContractStatus.Active));
            context.Contracts.Add(CreateContract(2, ContractStatus.Draft));
            context.SaveChanges();

            var service = new ContractService(GetInMemoryContext(), new FileService());

            var query = context.Contracts.AsQueryable();

            // Act
            var result = service.FilterContracts(query, null, null, ContractStatus.Active).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal(ContractStatus.Active, result.First().Status);
        }

        [Fact]
        public void DeleteContract_ShouldRemoveContract_AndDeleteFile()
        {
            // Arrange
            var context = GetInMemoryContext();
            var fileService = new FileService();
            var service = new ContractService(context, fileService);

            // Fake file
            var relativePath = "/uploads/contracts/testfile.pdf";
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","uploads","contracts");

            Directory.CreateDirectory(fullPath);

            var fileFullPath = Path.Combine(fullPath, "testfile.pdf");
            File.WriteAllText(fileFullPath, "dummy content");

            var contract = new ContractEntity
            {
                ContractId = 1,
                ServiceLevel = "Standard",
                Status = ContractStatus.Draft,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                ClientId = 1,
                AgreementFilePath = relativePath
            };

            context.Contracts.Add(contract);
            context.SaveChanges();

            // Act
            service.DeleteContract(contract);

            // Assert
            Assert.False(context.Contracts.Any());

            // Assert 
            Assert.False(File.Exists(fileFullPath));
        }
    }
}