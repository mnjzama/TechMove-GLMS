using Microsoft.EntityFrameworkCore;
using TechMoveAPI.Data;
using TechMoveAPI.Models;

/*
Author: Microsoft Learn
URL: https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
Date: [n.d]
Date Accessed: 01 June 2026

Author: Microsoft Learn
URL: https://learn.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/sql
Date: [n.d]
Date Accessed: 01 June 2026

Author: Julie Lerman
URL: https://learn.microsoft.com/en-us/archive/msdn-magazine/2018/august/data-points-deep-dive-into-ef-core-hasdata-seeding
Date: [n.d]
Date Accessed: 01 June 2026
*/
namespace TechMoveAPI.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Check if data already exists
            context.Database.Migrate();
            if (context.Clients.Any())
            {
                return;
            }

            // Clients
            var client1 = new Client
            {
                Name = "TechMove Demo Client 1",
                ContactDetails = "demo1@techmove.com",
                Region = "Gauteng"
            };

            var client2 = new Client
            {
                Name = "TechMove Demo Client 2",
                ContactDetails = "demo2@techmove.com",
                Region = "Limpopo"
            };

            context.Clients.AddRange(client1, client2);
            context.SaveChanges();

            // Contracts
            var contract1 = new ContractEntity 
            {
                ClientId = client1.ClientId,
                StartDate = DateTime.UtcNow.AddMonths(-2),
                EndDate = DateTime.UtcNow.AddMonths(10),
                ServiceLevel = "Standard",
                Status = ContractStatus.Active,
                AgreementFilePath = ""
            };

            var contract2 = new ContractEntity
            {
                ClientId = client2.ClientId,
                StartDate = DateTime.UtcNow.AddMonths(-1),
                EndDate = DateTime.UtcNow.AddMonths(6),
                ServiceLevel = "Express",
                Status = ContractStatus.Draft,
                AgreementFilePath = ""
            };

            context.Contracts.AddRange(contract1, contract2);
            context.SaveChanges();

            // Service Requests
            var request1 = new ServiceRequest
            {
                ContractId = contract1.ContractId,
                Description = "Initial system setup request",
                OriginalAmount = 1500,
                Currency = "USD",
                Cost = 27000,
                Status = ServiceRequestStatus.Pending
            };

            var request2 = new ServiceRequest
            {
                ContractId = contract1.ContractId,
                Description = "Monthly maintenance support",
                OriginalAmount = 500,
                Currency = "USD",
                Cost = 9000,
                Status = ServiceRequestStatus.Completed
            };

            context.ServiceRequests.AddRange(request1, request2);
            context.SaveChanges();
        }

    }
}