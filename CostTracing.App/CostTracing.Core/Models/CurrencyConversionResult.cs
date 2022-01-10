using System;
using System.Collections.Generic;
using System.Text;

namespace CostTracing.Core.Models
{
    public class CurrencyConversionResult
    {
        public string TargetSymbol { get; set; }

        public double Amount { get; set; }
    }
}
