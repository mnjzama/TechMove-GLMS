using TechMoveAPI.Domain.Contracts;
using TechMoveAPI.Models;

namespace TechMoveAPI.Domain.Factories
{
    public class ExpressContractFactory : IContractFactory
    {
        /*
            Author: Refactoring Guru
            URL: https://refactoring.guru/design-patterns/factory-method/csharp/example
            Date: [n.d]
            Date Accessed: 15 April 2026
        */
        public Contract CreateContract(string type)
        {
            // Return a new ExpressContract with default values
            return new ExpressContract
            {
                ServiceLevel = "Express",
                Status = ContractStatus.Draft
            };
        }
    }
}