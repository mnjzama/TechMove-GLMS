using TechMoveClient.Domain.Contracts;

namespace TechMoveClient.Domain.Factories
{
    /*
        Author: Refactoring Guru
        URL: https://refactoring.guru/design-patterns/factory-method/csharp/example
        Date: [n.d]
        Date Accessed: 15 April 2026
        */
    public interface IContractFactory
    {
        Contract CreateContract(string type);
    }
}