using TechMoveAPI.Services;

namespace TechMoveAPI.Domain.Strategy
{
     /*
        Author: Refactoring Guru
        URL: https://refactoring.guru/design-patterns/strategy/csharp/example
        Date: [n.d]
        Date Accessed: 13 April 2026
    */
    public class USDStrategy : ICurrencyStrategy
    {
        private readonly ExchangeRateService _service;

        // Constructor to inject the exchange rate service
        public USDStrategy(ExchangeRateService service)
        {
            _service = service;
        }

        // Method to convert the amount from USD to ZAR using the exchange rate service
        public async Task<decimal> ConvertAsync(decimal amount)
        {
            var rate = await _service.GetRateAsync("USD", "ZAR");
            return amount * rate;
        }
    }
}