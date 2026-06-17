namespace TechMoveAPI.Domain.Strategy
{
     /*
        Author: Refactoring Guru
        URL: https://refactoring.guru/design-patterns/strategy/csharp/example
        Date: [n.d]
        Date Accessed: 13 April 2026
        */
    public class ZARStrategy : ICurrencyStrategy
    {
        public Task<decimal> ConvertAsync(decimal amount)
        {
            return Task.FromResult(amount);
        }
    }
}