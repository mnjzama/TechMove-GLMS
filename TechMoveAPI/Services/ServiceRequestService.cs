using Microsoft.EntityFrameworkCore;
using TechMoveAPI.Domain.Contracts;
using TechMoveAPI.Domain.Factories;
using TechMoveAPI.Domain.Strategy;
using TechMoveAPI.Domain.Observers;
using TechMoveAPI.Models;
using TechMoveAPI.Data;

namespace TechMoveAPI.Services
{
    public class ServiceRequestService
    {
        private readonly AppDbContext _context;
        private readonly StrategyFactory _factory;

        public ServiceRequestService(AppDbContext context, StrategyFactory factory)
        {
            _context = context;
            _factory = factory;
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
        Date: 19 January 2023
        Date Accessed: 13 April 2026
        */
        public ServiceRequest? GetByIdWithContract(int id)
        {
            return _context.ServiceRequests
                .Include(r => r.Contract)
                .FirstOrDefault(r => r.ServiceRequestId == id);
        }

        /*
        Author: Refactoring Guru
        URL: https://refactoring.guru/design-patterns/observer/csharp/example
        Date: [n.d]
        Date Accessed: 13 April 2026
        */
        public void UpdateStatus(ServiceRequest entity, ServiceRequestStatus newStatus)
        {
            // Update entity
            entity.Status = newStatus;

            _context.SaveChanges();

            // Notify observers
            var subject = new ServiceRequestSubject
            {
                RequestId = entity.ServiceRequestId,
                Description = entity.Description,
                Cost = entity.Cost
            };

            subject.Attach(new NotificationService());
            subject.Attach(new DriverApp());

            subject.UpdateStatus(entity.Status.ToString());
        }

        public bool CanDelete(ServiceRequest request)
        {
            // A service request can only be deleted if it is in pending status
            return request.Status == ServiceRequestStatus.Pending;
        }

        /*
        Author: Refactoring Guru
        URL: https://refactoring.guru/design-patterns/strategy/csharp/example
        Date: [n.d]
        Date Accessed: 15 April 2026
        */
        public async Task<decimal> CalculateCostAsync(string currency, decimal amount)
        {
            // Get appropriate strategy based on the currency and calculate total cost
            var strategy = _factory.GetStrategy(currency);

            // Use strategy to calculate total cost for the service request
            var invoice = new Invoice();

            // Set strategy for the invoice and calculate total cost based on the provided amount
            invoice.SetStrategy(strategy);

            // Calculate total cost using the selected strategy and return the result
            return await invoice.CalculateTotalAsync(amount);
        }
    }
}