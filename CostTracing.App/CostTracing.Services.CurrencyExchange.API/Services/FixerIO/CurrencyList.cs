using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostTracing.Backend.CurrencyExchange.API.Services.FixerIO
{
    public class CurrencyList
    {
        public bool Success { get; set; }

        public DateTime Date { get; set; }
        
        public ulong TimeStamp { get; set; }

        public string Base { get; set; }


        public Dictionary<string, double> Rates { get; set;}
    }
}
