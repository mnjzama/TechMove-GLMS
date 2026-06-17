namespace TechMoveAPI.Domain.Strategy
{
    public class Invoice
    {
        /*
            Author: Refactoring Guru
            URL: https://refactoring.guru/design-patterns/strategy/csharp/example
            Date: [n.d]
            Date Accessed: 13 April 2026
        */
        private ICurrencyStrategy _strategy;

        // Constructor to set the initial strategy
        public void SetStrategy(ICurrencyStrategy strategy)
        {
            _strategy = strategy;
        }

        // Method to calculate the total amount using the current strategy
        public async Task<decimal> CalculateTotalAsync(decimal amount)
        {
            return await _strategy.ConvertAsync(amount);
        }
    }
}