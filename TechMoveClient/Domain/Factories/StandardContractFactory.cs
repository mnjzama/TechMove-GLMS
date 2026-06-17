using TechMoveClient.Domain.Contracts;
using TechMoveClient.Models;

namespace TechMoveClient.Domain.Factories
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
            return new StandardContract
            {
                ServiceLevel = "Standard",
                Status = ContractStatus.Draft
            };
        }
    }
}