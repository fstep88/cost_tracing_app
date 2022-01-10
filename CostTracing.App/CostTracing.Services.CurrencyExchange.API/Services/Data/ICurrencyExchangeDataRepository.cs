using System.Threading.Tasks;

namespace CostTracing.Backend.CurrencyExchange.API.Services.Data
{
    public interface ICurrencyExchangeDataRepository
    {
        Task<CurrencyList> GetCurrencyExchangeData();
    }
}
