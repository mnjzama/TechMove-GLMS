using TechMoveAPI.Domain.Contracts;
using TechMoveAPI.Domain.Factories;
using TechMoveAPI.Models;
using TechMoveAPI.Data;

namespace TechMoveAPI.Services
{
    public class ContractService
    {
        private readonly AppDbContext _context;
        private readonly FileService _fileService;

        public ContractService(AppDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public bool CanCreateServiceRequest(ContractEntity contract)
        {
            // A service request can only be created if the contract is active
            return contract.Status == ContractStatus.Active;
        }

        public bool CanDelete(ContractEntity contract)
        {
            // A contract can only be deleted if it is in draft status
            return contract.Status == ContractStatus.Draft;
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.delete?view=net-10.0
        Date: [n.d]
        Date Accessed: 19 April 2026
        */
        public void DeleteContract(ContractEntity contract)
        {
            _fileService.DeleteFile(contract.AgreementFilePath);

            _context.Contracts.Remove(contract);
            _context.SaveChanges();
        }

        /*
        Author: Refactoring Guru
        URL: https://refactoring.guru/design-patterns/factory-method/csharp/example
        Date: [n.d]
        Date Accessed: 14 April 2026
        */
        public Contract CreateContract(string serviceLevel)
        {
            // Use the factory to create the appropriate contract based on service level
            IContractFactory factory = serviceLevel == "Express"
                ? new ExpressContractFactory()
                : new StandardContractFactory();

            return factory.CreateContract(serviceLevel);
        }

        public void UpdateStatus(ContractEntity contract, ContractStatus status)
        {
            // Update the contract status
            contract.Status = status;
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.linq.queryable.where
        Date: [n.d]
        Date Accessed: 14 April 2026
        */
        public IQueryable<ContractEntity> FilterContracts(IQueryable<ContractEntity> query, DateTime? startDate, DateTime? endDate, ContractStatus? status)
        {
            // Filter contracts based on provided criteria
            if (startDate.HasValue)
                query = query.Where(c => c.StartDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.EndDate <= endDate.Value);

            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);

            return query;
        }

        public bool IsValidContractDates(DateTime startDate, DateTime endDate)
        {
            // A contract is valid if the end date is on or after the start date
            return endDate >= startDate;
        }

        public bool HasValidDuration(DateTime startDate, DateTime endDate)
        {
            // A contract must have a duration of at least 1 day
            return (endDate - startDate).TotalDays >= 1;
        }
    }
}