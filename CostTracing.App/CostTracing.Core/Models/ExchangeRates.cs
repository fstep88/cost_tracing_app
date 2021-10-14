using System;
using System.Collections.Generic;
using System.Text;

namespace CostTracing.Core.Models
{
    public class ExchangeRates
    {
        public ExchangeRates()
        {
            Rates = new Dictionary<string, object>();
        }

        public string Base { get; set; }

        public DateTime Date { get; set; }

        public Dictionary<string, object> Rates { get; set; }
    }
}
