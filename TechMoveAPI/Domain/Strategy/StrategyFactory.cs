using TechMoveAPI.Services;

namespace TechMoveAPI.Domain.Strategy
{
     /*
        Author: Refactoring Guru
        URL: https://refactoring.guru/design-patterns/strategy/csharp/example
        Date: [n.d]
        Date Accessed: 13 April 2026
    */
    public class StrategyFactory
    {
        private readonly ExchangeRateService _service;

        // Constructor to inject the exchange rate service
        public StrategyFactory(ExchangeRateService service)
        {
            _service = service;
        }

        public ICurrencyStrategy GetStrategy(string currency)
        {
            // Factory method to return the appropriate strategy based on the currency
            return currency switch
            {
                "USD" => new USDStrategy(_service),
                "EUR" => new EURStrategy(_service),
                _ => new ZARStrategy()
            };
        }
    }
}