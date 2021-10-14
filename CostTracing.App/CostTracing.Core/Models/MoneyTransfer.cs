using System;
using System.Collections.Generic;
using System.Text;

namespace CostTracing.Core.Models
{
    public class MoneyTransfer : MetaData
    {

        public Identifier Recipient { get; set; }

        public Identifier Donator { get; set; }

        public bool Positiv { get; set; }

        public Identifier CurrencyIdentifier { get; set; }
        
        public double Amount { get; set; }
    }
}
