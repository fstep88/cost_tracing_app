using CostTracing.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CostTracing.Backend.CurrencyExchange.API.Services.Data
{
    class CurrencyDataFileService : ICurrencyDataService
    {
        public async Task<IEnumerable<Currency>> GetCurrencyDataAsync()
        {
            using (var fs = new FileStream(DataPath, FileMode.Open))
            {
                return await System.Text.Json.JsonSerializer.DeserializeAsync<List<Currency>>(fs);
            }
            
        }


        protected string CurrentListPath => Path.Combine(Directory.GetCurrentDirectory(), "Services", "Data", "CurrentList.json");

        protected string DataPath = Path.Combine(Directory.GetCurrentDirectory(), "Services", "Data", "CurrencyData.json");

        public async Task<CurrencyList> GetCurrentListAsync()
        {
            if (!File.Exists(CurrentListPath))
            {
                return null;
            }

            using (var fs = new FileStream(CurrentListPath, FileMode.Open))
            {
                return await System.Text.Json.JsonSerializer.DeserializeAsync<CurrencyList>(fs);
            }
        }

        public async Task SetCurrentListAsync(CurrencyList list)
        {
            await File.WriteAllTextAsync(DataPath, System.Text.Json.JsonSerializer.Serialize(list, typeof(CurrencyList)));
        }
    }
}
