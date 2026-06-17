using TechMoveAPI.Domain.Contracts;

namespace TechMoveAPI.Domain.Factories
{
    /*
        Author: Refactoring Guru
        URL: https://refactoring.guru/design-patterns/factory-method/csharp/example
        Date: [n.d]
        Date Accessed: 15 April 2026
    */
    public interface IContractFactory
    {
        // Factory method to create a contract based on the type
        Contract CreateContract(string type);
    }
}