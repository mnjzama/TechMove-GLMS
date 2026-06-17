using TechMoveAPI.Domain.Contracts;
using TechMoveAPI.Models;

namespace TechMoveAPI.Domain.Factories
{
    public class StandardContractFactory : IContractFactory
    {
        /*
            Author: Refactoring Guru
            URL: https://refactoring.guru/design-patterns/factory-method/csharp/example
            Date: [n.d]
            Date Accessed: 15 April 2026
        */
        public Contract CreateContract(string type)
        {
            // Return a new StandardContract with default values
            return new StandardContract
            {
                ServiceLevel = "Standard",
                Status = ContractStatus.Draft
            };
        }
    }
}