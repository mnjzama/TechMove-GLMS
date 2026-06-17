/*
    Author: Refactoring Guru
    URL: https://refactoring.guru/design-patterns/strategy/csharp/example
    Date: [n.d]
    Date Accessed: 13 April 2026
*/
namespace TechMoveAPI.Domain.Strategy
{
    // Strategy interface that defines the method for currency conversion
    public interface ICurrencyStrategy
    {
        Task<decimal> ConvertAsync(decimal amount);
    }
}